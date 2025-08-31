# Use the official Node.js image as base
FROM node:20-alpine

# Set working directory
WORKDIR /app

# Copy package files first for better layer caching
COPY package*.json ./

# Install dependencies
RUN npm ci --only=production=false

# Copy source code (will be overridden by volume mount in development)
COPY . .

# Expose port 4200
EXPOSE 4200

# Start the development server with host binding for Docker
CMD ["npm", "run", "start:docker"]
