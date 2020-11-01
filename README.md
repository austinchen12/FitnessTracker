FitnessTracker is a program acting as a client, first authenticating with a username/password combination and then writing or reading to the server-side database. Uses HttpClient and api calls from ApiHelperMethods to FitnessTrackerApi. FitnessTrackerApi accesses a local SQLite database and writes/reads entries from the database.

In FitnessTrackerApi, there is config.txt which specifies the full path of the database. Database tables are described by the corresponding models.
