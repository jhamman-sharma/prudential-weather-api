# prudential-weather-api

Used to reterive Weather information of cities. Internally this serivce used Open Weather REST API to reterive up to date information.

Service have only one end point as follow -
POST - http://base-url/api/Weather

## Usage - 

POST the request through REST Client like POSTMAN with the following example JSON body

```
[
    {
        "Id": 1275339,
        "Name": "Mumbai"
    },
    {
        "Id": 2988507,
        "Name": "Paris"
    },
    {
        "Id": 1273294,
        "Name": "Delhi"
    }
]
```

### Note -

1. I have also attached POSTMAN collection with solution which you can import in POSTMAN and directly hit the service
2. This service also loged retrived Weather information in JSON file at windows drive C://WeatherLog location (Path is configurable through config file).
