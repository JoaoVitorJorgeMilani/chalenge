FROM rabbitmq:latest AS mottu-rabbitmq-image

RUN rabbitmq-plugins enable --offline rabbitmq_mqtt rabbitmq_federation_management rabbitmq_stomp

ENV RABBITMQ_DEFAULT_USER=guest
ENV RABBITMQ_DEFAULT_PASS=guest

EXPOSE 5672
EXPOSE 15672