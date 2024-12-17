ALTER TABLE health_check.session_user
    ADD COLUMN connection_id TEXT;

ALTER TABLE health_check.session
    ADD COLUMN host_connection_id TEXT;
