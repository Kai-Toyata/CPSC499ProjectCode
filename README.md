# TeejLab Project

## Introduction

This project is to simulate API resource usage for rate-limit testing.


## Prerequisites

Before you begin, ensure you have the following installed:
- [Git](https://git-scm.com/downloads) for version control.
- [Postman](https://www.postman.com/downloads/) for API testing and interaction.
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) for running the API itself.

## Installation

**Clone the Repository**
   ```
   git clone https://github.com/Kai-Toyata/CPSC499ProjectCode.git
   cd CPSC499ProjectCode
   ```

## Running the Project

```
dotnet run
```

## Using Postman for API Testing

### Importing the Collection into Postman

1. **Open Postman**

2. **Import the Collection**
   - Click the `Import` button in the top left corner.
   - Select `File` and click `Upload Files`.
   - Browse to the location of `TeejLab.postman_collection.json` and select it.
   - Click `Import` to add the collection to your Postman.

3. **Using the Collection**
   - Once imported, you will see `TeejLab` listed in your Collections.
   - Expand the collection to see the available API requests.
   - Select an API request to view its details and send a request.

### Testing APIs

- You can run repeated calls and custom tests by right-clicking TeejLab under Collections and then clicking "Run Collection".

## License

This project is licensed under the [GNUv3 License](LICENSE).
