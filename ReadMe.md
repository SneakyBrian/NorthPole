# North Pole #

## A HTTP Post Data Signing and Verification Service ##

North Pole provides 2 HTTP end points that except any arbitrary POST data.

`/elf` is the end point that signs the data with a timestamp and a HMAC

`/santa` is the end point that verifies the values returned by `/elf`

`/santa` responds with:

- `NICE` if the POST data is verified successfully
- `NAUGHTY` if the POST data HMAC does not match the specified HMAC
- `NOTREADY (Milliseconds to ready time)` if the timestamp is too soon, i.e. is not older than the minimum age specified in the config

The service has 2 configuration items:

1. A minimum age that the timestamps must be before they will pass verification by `/santa`
2. A super secret key that is combined with the user-specified key in order to generate the HMAC

See `test.html` for example usage.

