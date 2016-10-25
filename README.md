[![Build status](https://ci.appveyor.com/api/projects/status/30oigl97pus96de3/branch/master?svg=true)](https://ci.appveyor.com/project/Development-LetMe/transactionalemail/branch/master)

# TransactionalEmail
API to send and receive transactional email

# Security
In order for this service to be able to interact with the google apps domain, it must be configured in the Google Developer Console with the approropriate service account and certificate generated, downloaded and installed.

In the google console -> create a new project. Once created go to credentials -> Create credentials -> OAuth 2.0 Client Id. Give it a name.

Once created, go to Create credentials -> Service Account Key. Select the service account name you just created and choose P12 as the key type. Download the certificate and keep it safe, it will need to be either installed via the Azure portal or deployed alongside the code.

# Routing
Rather than monitoring all agent mailboxes, it is easer to route all email into a central collector mailbox and monitor that. This is normally achieved using a specific alias e.g. @mail.letme.com

# Alias Setup
After creating the required alias domain, and adding the alias to Google user, the final step that we alwasy forget is to configure the alias in the agents email inbox. This can either be done via GAM or in the GMAIL web UI.
