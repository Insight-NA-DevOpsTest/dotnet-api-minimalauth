ARG VARIANT=8.0.8-bookworm-slim-amd64
FROM mcr.microsoft.com/dotnet/aspnet:${VARIANT}

ARG VARIANT
RUN if id "ubuntu" &>/dev/null; then \
        echo "Deleting user 'ubuntu'  for $VARIANT" && userdel -f -r ubuntu || echo "Failed to delete ubuntu user for $VARIANT"; \
    else \
        echo "User 'ubuntu' does not exist for $VARIANT"; \ 
    fi
