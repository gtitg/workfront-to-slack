# workfront-to-slack
Checks for updates from a workfront (formerly AtTask) team and pushes them to a slack channel. 

This is a work in progress.

The app currently gets all of the users in your specified team, and gets the most recent 20 updates for each user.

It currenty keeps track of which updates it has already sent to slack in a local text file.  

We also make sure that an update occurred today before sending it to slack.

We format the updates as slack attachments so that the project name, task name, and updater name are displayed nicely:

![Screenshot of workfront update appearing as a formatted slack attachment](/screenshots/workfront_slack_attachment.png?raw=true)

-- Stephen