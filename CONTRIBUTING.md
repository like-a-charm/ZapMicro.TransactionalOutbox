Contributing to ZapMicro.TransactionalOutbox

We love your input! We want to make contributing to this project as easy and transparent as possible, whether it's:

    Reporting a bug
    Discussing the current state of the code
    Submitting a fix
    Proposing new features
    Becoming a maintainer

We Develop with Github

We use github to host code, to track issues and feature requests, as well as accept pull requests.
We Use Github Flow, So All Code Changes Happen Through Pull Requests

Pull requests are the best way to propose changes to the codebase (we use Github Flow). We actively welcome your pull requests:

    Fork the repo and create your branch from main.
    If you've added code that should be tested, add tests.
    If you've changed APIs, update the documentation.
    Ensure the test suite passes.
    Make sure your code lints.
    Issue that pull request!

Run Unit Tests

To execute the unit tests execute the following

dotnet test --settings settings/coverlet-runsettings.xml --logger trx --results-directory "test-results"
reportgenerator "-reports:test-results/**/*.opencover.xml" "-targetdir:test-report"

The reports will be available in the test-result folder