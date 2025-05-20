-- This schema is generated based on the current DBContext. Please check the class Seeder to see.
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'plants') THEN
        CREATE SCHEMA plants;
    END IF;
END $EF$;


CREATE TABLE plants.plant (
    id text NOT NULL,
    plant_name text NOT NULL,
    plant_type text NOT NULL,
    moisture_level real NOT NULL,
    moisture_threshold real NOT NULL,
    is_auto_watering_enabled boolean NOT NULL,
    CONSTRAINT plant_pkey PRIMARY KEY (id)
);


CREATE TABLE plants."user" (
    id text NOT NULL,
    email text NOT NULL,
    hash text NOT NULL,
    salt text NOT NULL,
    role text NOT NULL,
    CONSTRAINT user_pkey PRIMARY KEY (id)
);


CREATE TABLE plants.user_plant (
    user_id text NOT NULL,
    plant_id text NOT NULL,
    is_owner boolean NOT NULL,
    CONSTRAINT user_plant_pkey PRIMARY KEY (user_id, plant_id),
    CONSTRAINT "FK_user_plant_plant_plant_id" FOREIGN KEY (plant_id) REFERENCES plants.plant (id) ON DELETE CASCADE,
    CONSTRAINT "FK_user_plant_user_user_id" FOREIGN KEY (user_id) REFERENCES plants."user" (id) ON DELETE CASCADE
);


CREATE TABLE plants.watering_log (
    id text NOT NULL,
    plant_id text NOT NULL,
    triggered_by_user_id text,
    timestamp timestamp with time zone NOT NULL,
    method integer NOT NULL,
    CONSTRAINT watering_log_pkey PRIMARY KEY (id),
    CONSTRAINT "FK_watering_log_plant_plant_id" FOREIGN KEY (plant_id) REFERENCES plants.plant (id) ON DELETE CASCADE,
    CONSTRAINT "FK_watering_log_user_triggered_by_user_id" FOREIGN KEY (triggered_by_user_id) REFERENCES plants."user" (id) ON DELETE SET NULL
);


CREATE INDEX "IX_user_plant_plant_id" ON plants.user_plant (plant_id);


CREATE INDEX "IX_watering_log_plant_id" ON plants.watering_log (plant_id);


CREATE INDEX "IX_watering_log_triggered_by_user_id" ON plants.watering_log (triggered_by_user_id);


