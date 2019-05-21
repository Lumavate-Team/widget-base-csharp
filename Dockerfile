FROM quay.io/lumavate/edit:base as editor

COPY supervisord.conf /etc/supervisor/conf.d

#FROM mcr.microsoft.com/dotnet/core/runtime-deps:2.2-alpine3.9

FROM debian:stretch-slim

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
# .NET Core dependencies
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu57 \
        liblttng-ust0 \
        libssl1.0.2 \
        libstdc++6 \
        zlib1g

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true



COPY --from=editor /editor /editor
COPY --from=editor /logs /logs
COPY --from=editor /etc/supervisor /etc/supervisor/
COPY --from=editor /edit_requirements.txt /edit_requirements.txt

RUN apt-get install -y --no-install-recommends build-essential checkinstall
RUN apt-get install -y --no-install-recommends \
      libreadline-gplv2-dev \
      libncursesw5-dev \
      libssl-dev \
      libsqlite3-dev \
      tk-dev \
      libgdbm-dev \
      libc6-dev \
      libbz2-dev \
      libffi-dev \
      zlib1g-dev \
      wget

RUN cd /usr/src \
      && wget https://www.python.org/ftp/python/3.7.3/Python-3.7.3.tgz \
      && cd Python-3.7.3 \
      && ./configure --enable-optimizations \
      && make install

RUN python --version

# Disable the invariant mode (set in base image)
RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.2.204

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='025e2b52cb3b082583ae7071d414db3725989ba7c16b28fb9e5ddf0427f713f0e8b152aadd87137c1e6e2dc64403a7c7b697ec992f00507f5dbf17f1f4f4eb71' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz

# Enable correct mode for dotnet watch (only mode supported in a container)
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip

# Trigger first run experience by running arbitrary cmd to populate local package cache
RUN dotnet help

EXPOSE 5000

COPY supervisord.conf /etc/supervisor/conf.d

WORKDIR /app
COPY ./app /app/

CMD ["supervisord", "-c", "/etc/supervisor/supervisord.conf"]
