# MessageHub
A .NET project to solve a real problem when delivering messages to group of people through different means. 

export GOOGLE_APPLICATION_CREDENTIALS="./firebase-service-account.json"

TOKEN=$(curl -s -X POST "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=$WEB_API_KEY" \
  -H "Content-Type: application/json" \
  -d '{"email":"me@test.com","password":"secret123","returnSecureToken":true}' | jq -r .idToken)

## Notifications

When an event is created, every client in the event's groups gets a message by email and/or SMS.
Sending happens in the background, so creating an event stays fast even for large groups, and
each message's delivery result is stored in Firestore.

```
POST /api/events
   └─ EventsService.CreateAsync
        ├─ save the event
        ├─ GroupsService.GetGroupClientsAsync(event.Groups)   → recipients
        └─ NotificationsRepository.AddRangeAsync(...)          → one Pending notification
                                                                 per recipient and channel

NotificationWorker (BackgroundService, polls every few seconds)
   └─ NotificationsRepository.GetPendingAsync()
        └─ INotificationSender for the channel
             ├─ success → Status = Sent, SentAt = now
             └─ failure → Attempts++, LastError = message;
                          Status = Failed once Attempts reaches the retry limit
```

### Status

| Done | Piece |
|---|---|
| ✅ | `Notifications` model ([Models/Notifications.cs](Models/Notifications.cs)), stored in the `notifications` collection |
| ✅ | `NotificationsRepository` ([Repositories/NotificationsRepository.cs](Repositories/NotificationsRepository.cs)): batched `AddRangeAsync`, `GetPendingAsync`, `GetByEventIdAsync` |
| ✅ | Register `INotificationsRepository` in `Program.cs` |
| ✅ | `EventsService.CreateAsync` creates the notifications |
| ⬜ | `INotificationSender` with `LoggingSmsSender` and `SmtpEmailSender` |
| ⬜ | `NotificationWorker` background service |
| ⬜ | `GET /api/events/{id}/notifications` endpoint for delivery history |

### Rules for the pieces still to build

- **Creating notifications:** create one notification per client *and* channel. A client with
  both an email and a phone gets two. Skip clients with neither. Copy the email address or
  phone number into `Recipient`, so the history stays accurate if the client is edited later.
  Use the event title as `Subject` and the event text as `Body`.
- **Sender interface:** `INotificationSender` exposes `Channel` and
  `SendAsync(recipient, subject, body, ct)`. Register one implementation per channel. The worker
  picks the sender whose `Channel` matches the notification.
- **Worker:** process notifications one at a time, and save each one right after sending it
  (`UpdateAsync`), so a crash doesn't resend messages that already went out. Leave a failed
  notification `Pending` until it reaches the retry limit (e.g. 3), then mark it `Failed`.
- **Local testing without accounts:** `LoggingSmsSender` writes the SMS to the console.
  `SmtpEmailSender` can point at [Mailpit](https://mailpit.axllent.org/), a local mail catcher:
  `docker run -p 8025:8025 -p 1025:1025 axllent/mailpit`. The SMTP server is `localhost:1025`,
  and caught emails show up at http://localhost:8025.
- **Known limits:** `GetPendingAsync` has no ordering, because sorting by date together with the
  status filter would need a Firestore composite index. With more than one worker, two workers
  could pick up the same notification. A single worker is fine for this project. If saving the
  notifications fails after the event is saved, the event exists without notifications.
