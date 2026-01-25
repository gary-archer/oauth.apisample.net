FROM mcr.microsoft.com/dotnet/aspnet:10.0

ARG ARCH
WORKDIR /usr/api
COPY --chown=10001:10000 bin/Release/net10.0/linux-$ARCH/publish/*  /usr/api/
COPY --chown=10001:10000 data/*                                     /usr/api/data/

RUN groupadd --gid 10000 apiuser \
  && useradd --uid 10001 --gid apiuser --shell /bin/bash --create-home apiuser
USER 10001

CMD ["dotnet", "finalapi.dll"]
