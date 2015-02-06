# workfront-to-slack
Checks for updates from a workfront (formerly AtTask) team and pushes them to a slack channel. 

This is a work in progress.  The app currently only searches for your team in workfront and prints out the most recent 20 updates to the console.

Next step is to capture these and send them to slack.  We'll need a good way of keeping track of which updates we have already sent.  I'm thinking SQL server, but that seems like too much of a burden to put on other people who may want to use this system.

-- Stephen