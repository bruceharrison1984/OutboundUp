# OutboundUp

![Screenshot](outboundup-screenshot.png)

## Description

OuboundUp is a real-time, outbound internet connection monitor and speed tester. It is fully containerized, and can be run with or without full Sqlite database persistence.

- Ookla Speed Test
- Sqlite persistence layer
- Angular front-end
- .net core 6 backend
- Quart.net for job scheduling

## Docker

```sh
## Run from the repo root directory to build the container locally
> docker build -f OutboundUp/Dockerfile . -t outbound-up` .

## Run the container
> docker run -d -p 8080:80 outbound-up

## Access the UI through http://localhost:80
```

## Web Hooks

Generic webhooks are supported and allow you to make POST requests to a given endpoint after each Speed Test has concluded. The body of the request will contain the JSON encoded results of the speed test.
