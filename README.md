# workfront-to-slack
Checks for updates from a workfront (formerly AtTask) team and pushes them to a slack channel. 

This is a work in progress.

The app currently gets all of the users in your specified team, and gets the most recent 20 updates for each user.

It currenty keeps track of which updates it has already sent to slack in a local text file.  

The idea is that eventually we will query workfront only for updates that have happened in the past day, and then we will clear the text file of any updates older than 1 day.  This removes the need for any complex database storage which seems like overkill.

Next step is to format the messages as Slack attachements, so that the task name and project name for each user's update can also be listed in a readable way.

-- Stephen