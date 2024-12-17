ALTER TABLE health_check.session
    ADD COLUMN is_started boolean NOT NULL DEFAULT false;
