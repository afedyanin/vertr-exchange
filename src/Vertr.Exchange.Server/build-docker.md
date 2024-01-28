## Build image

```
docker build -t afedyanin/vertr-exchange:v1.0.1 .
```

## Pull docker Inamge

```
docker pull afedyanin/vertr-exchange:v1.0.1
```


## Run container
```
docker run --rm -d -p 5000:8080 afedyanin/vertr-exchange:v1.0.1
```

## Publish Container

```
docker login --username=afedyanin
docker images
docker tag 12dcfc901c3f afedyanin/vertr-exchange:v1.0.1
docker push afedyanin/vertr-exchange:v1.0.1
```

## Delete Image

```
docker image rm 12dcfc901c3f
```
