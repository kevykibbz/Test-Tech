# Development with Docker Volume Mounting

This setup provides **live reload** during development using Docker volume mounting.

## How it works:

1. **Source Code Mounting**: Your local `WebApp` folder is mounted into the Docker container at `/app`
2. **Node Modules Volume**: A separate Docker volume is used for `node_modules` to maintain performance
3. **Live Reload**: Angular CLI watches for file changes and automatically rebuilds
4. **Hot Reload**: Browser automatically refreshes when changes are detected

## Development Workflow:

1. Start the Aspire AppHost (F5 in Visual Studio)
2. Make changes to any TypeScript, HTML, or CSS files in the `WebApp` folder
3. Changes are automatically detected and the app rebuilds
4. Browser refreshes automatically to show your changes

## Files and Configuration:

- `Dockerfile.dev` - Development-optimized Docker image
- Volume mount configuration in `TechTest.AppHost/Program.cs`
- Angular CLI configured with `--host 0.0.0.0` and `--poll=2000` for Docker compatibility

## Benefits:

- ✅ No Node.js installation required on your machine
- ✅ Fast development feedback loop
- ✅ Consistent development environment
- ✅ Works the same across different machines
- ✅ Isolated dependencies in container

## Troubleshooting:

If you don't see live reload working:
1. Ensure Docker Desktop is running
2. Restart the Aspire AppHost
3. Check the Aspire dashboard for any container errors
4. Verify that the WebApp folder is being mounted correctly
