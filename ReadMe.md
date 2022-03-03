# BT Tech Challenge

Write a solution in a programming language of your choice that can generate a one-time password. The input should be the following two pieces information: User Id and Date time. Every generated password should be valid for up to 30 seconds.

You are free to use a Web, MVC or Console project in order to accomplish the requirement.

## Instructions

Build and run the project using Visual Studio 2019 on Windows 10 (environment that it was developed on)

The solution was not tested on other environments however it should in theory work just the same.

The easiest way to test the API is using the swagger UI which opens automatically in a browser, if that does not happen go to 
`https://localhost:44312/swagger/index.html` (check that the SSL port in launchSettings.json is 44312)

Alternatively you can use curl to perform the following operations: 

### Create OTP
```bash
curl -X 'POST' \
  'https://localhost:44312/OneTimePassword?userId=407afd7d-46bb-467a-a35b-5637fba6bd29' \
  -H 'accept: text/plain' \
  -d ''
  ```
### Create OTP with Date (as per requirement)
```bash
curl -X 'POST' \
  'https://localhost:44312/OneTimePassword/withDate?userId=407afd7d-46bb-467a-a35b-5637fba6bd29&dateTime=2022-03-03T20%3A01%3A41.7861914%2B00%3A00' \
  -H 'accept: text/plain' \
  -d ''
```

### Consume the OTP
```bash
curl -X 'PUT' \
  'https://localhost:44312/OneTimePassword?userId=407afd7d-46bb-467a-a35b-5637fba6bd29&passwordInput=pleaseUseActualOTPHere' \
  -H 'accept: text/plain'
```

## Assumptions made during development:

- The generated one time password needs a method of consumption so I've added an appropriate endpoint for that purpose.

## Other

I've opted to keep the project structure simple as there's not a lot of code to make a pragmatic decision to break it into appropriate components, there is more separation I could have done however it was simply unecessary.