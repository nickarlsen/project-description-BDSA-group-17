# Use Cases

Use Case: Commit Frequency Mode
=================================
**Actors**: GitInsight User

**Scope**: The application

**Purpose**: Describe how the Commit Frequency Mode is used and executed. 

**Type**: Primary (Secondary, Optional)

**Overview**: This use case depicts one of the primary functonalities of the application, which is collecting and displaying data about a github repository in "Commit Frequency Mode", which means it outputs the amount of commits for every day a commit was made. 

Typical course of events:
----------------------

| Actor Action | System Response |
|:--------------|:----------------|
| 1. The GitInsight User startup the application| 2. The application promps the user to provide a path to a Git repository that resides in a local directory |
| 3. The GitInsight User provides the path to a repository on their PC | 4. The application promps the GitInsight User about which mode they want to run the application in; Commit Frequency Mode or Commit Author Mode |
|5. The GitInsight User selects the Commit Frequency Mode | 6. The application returns an output which depicts the amount of commits to the Git Repository for each day where a commit was made |
|7. The GitInsight User see the output????  | 8. The application is now done ????? |


Use Case: Commit Author Mode
=================================
**Actors**: GitInsight User

**Scope**: The application

**Purpose**: Describe how the Commit Author Mode is used and executed. 

**Type**: Primary (Secondary, Optional)

**Overview**: This use case depicts one of the primary functonalities of the application, which is collecting and displaying data about a github repository in "Commit Author Mode", which means it outputs for every author who has made a commit, everyday they have made a commit and the amount of commits for each of these days. 

Typical course of events:
----------------------

| Actor Action | System Response |
|:--------------|:----------------|
| 1. The GitInsight User startup the application| 2. The application promps the user to provide a path to a Git repository that resides in a local directory |
| 3. The GitInsight User provides the path to a repository on their PC | 4. The application promps the GitInsight User about which mode they want to run the application in; Commit Frequency Mode or Commit Author Mode |
|5. The GitInsight User selects the Commit Author Mode | 6. The application returns an output which depicts every author on the repository, and the amount of commits they have made for every day they made a commit |
|7. The GitInsight User see the output????  | 8. The application is now done ????? |

________________________________________________________________________________________

# Template:

Use Case: Name of use case.
=================================
**Actors**: Actor

**Scope**: Software system

**Purpose**: Intention of the use case.

**Type**: Primary (Secondary, Optional)

**Overview**: A brief description of what happens in this use case.

Typical course of events:
----------------------

| Actor Action | System Response |
|:--------------|:----------------|
| 1. This use case begins when Actor wants to initiate an event.| |
| 2. The Actor does something... | 3. The system determines something or responds... |
|4. ||
|5. | 6. |