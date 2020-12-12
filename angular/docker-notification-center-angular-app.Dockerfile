FROM node:alpine AS notification-center-angular-app
WORKDIR /app
COPY . .
RUN npm ci && npm run build

# stage 2

FROM nginx:alpine
COPY --from=notification-center-angular-app /app/dist/ /usr/share/nginx/html
EXPOSE 80
