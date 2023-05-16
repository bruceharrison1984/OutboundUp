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

### Pull pre-built from Github Packages

```sh
> docker pull ghcr.io/bruceharrison1984/outboundup:latest
> docker run -d -p 8080:80 outboundup
```

### Build image locally from source

```sh
## Run from the repo root directory to build the container locally
> docker build -f OutboundUp/Dockerfile . -t outboundup` .

## Run the container
> docker run -d -p 8080:80 outboundup

## Access the UI through http://localhost:80
```

## Web Hooks

Generic webhooks are supported and allow you to make POST requests to a given endpoint after each Speed Test has concluded. The body of the request will contain the JSON encoded results of the speed test.

You can use something like [Webhook.site](https://webhook.site/) to test out this functionality.

```json
// example webhook payload
{
  "Id": 72,
  "UnixTimestampMs": 1684266441000,
  "IsSuccess": true,
  "ServerHostName": "smo.speedtest.sbcglobal.net",
  "UploadLatencyAverage": 21.696,
  "UploadLatencyHigh": 58.255,
  "UploadLatencyLow": 8.156,
  "DownloadLatencyAverage": 31.722,
  "DownloadLatencyHigh": 46.654,
  "DownloadLatencyLow": 3.925,
  "PingAverage": 3.21,
  "PingHigh": 3.459,
  "PingLow": 3.146,
  "DownloadSpeed": 176.72,
  "UploadSpeed": 906.4
}
```
