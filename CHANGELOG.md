# Changelog - NetWork

All notable changes to the NetWork project will be documented in this file.

## Table of Contents

- [Unreleased](#unreleased)
- [Released](#released)
    - [v0.1.0 - 2023-10-17](#v010---2023-10-17)

## Unreleased
- Basic communication
  - Update **main** methods in both Sender and Receiver to be able to exchange a message through UDP connection
- Basic serialization
  - Create **Packet** class 
    - Add fields required in a typical packet, including TCP required flags SYN, ACK, FIN, RST
    - Add **Serialize** method
    - Add **Deserialize** method
- Solution creation
  - Create empty dotnet solution
  - Add projects to solution
    - Add **Receiver** project
    - Add **Sender** project
    - Add **Shared** project

## Released

### v0.1.0 - 2024-12-04



