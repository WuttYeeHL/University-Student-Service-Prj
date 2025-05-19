# Stage 1: Build the Angular app
FROM node:18-alpine AS build

WORKDIR /app
COPY . .

# Install Angular CLI globally
RUN npm install -g @angular/cli

# Install project dependencies
RUN npm install

# Build the app for production (pass project name and configuration)
RUN ng build student-portal --configuration production

# Stage 2: Serve the app with nginx
FROM nginx:alpine

COPY --from=build /app/dist/student-portal/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
