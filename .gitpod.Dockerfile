FROM gitpod/workspace-full-vnc

RUN sudo apt-get update && \
    sudo apt-get install -y dotnet-sdk-8.0   # Install the .NET SDK

