-- Glo_AppointmentAssignmentEventType_Seed.sql
-- Idempotent seed for glo.GloAppointmentAssignmentEventType.
-- Run after Setup migration AddGloAppointmentAssignmentEventType.

INSERT INTO glo."GloAppointmentAssignmentEventType"
(
    "EventTypeId",
    "Code",
    "Name",
    "Description",
    "DisplayOrder",
    "IsActive",
    "CreatedOn"
)
SELECT
    v."EventTypeId",
    v."Code",
    v."Name",
    v."Description",
    v."DisplayOrder",
    true,
    timezone('utc', now())
FROM (
    VALUES
        (1::smallint,  'ON_DUTY',                'On Duty',                'Technician started duty.', 1::smallint),
        (2::smallint,  'OFF_DUTY',               'Off Duty',               'Technician ended duty.', 2::smallint),
        (3::smallint,  'DISPATCH',               'Dispatch',               'Technician dispatched to appointment.', 3::smallint),
        (4::smallint,  'ARRIVE',                 'Arrive',                 'Technician arrived at job site.', 4::smallint),
        (5::smallint,  'COMPLETE',               'Complete',               'Technician completed appointment work.', 5::smallint),
        (6::smallint,  'CHECK_IN',               'Check-In',               'Technician checked in.', 6::smallint),
        (7::smallint,  'CHECK_OUT',              'Check-Out',              'Technician checked out.', 7::smallint),
        (8::smallint,  'LUNCH_START',            'Lunch Start',            'Technician started lunch break.', 8::smallint),
        (9::smallint,  'LUNCH_END',              'Lunch End',              'Technician ended lunch break.', 9::smallint),
        (10::smallint, 'PAUSE_START',            'Pause Start',            'Technician started pause.', 10::smallint),
        (11::smallint, 'PAUSE_END',              'Pause End',              'Technician ended pause.', 11::smallint),
        (12::smallint, 'DOCUMENTATION_COMPLETE', 'Documentation Complete', 'Technician completed documentation.', 12::smallint)
) AS v("EventTypeId", "Code", "Name", "Description", "DisplayOrder")
WHERE NOT EXISTS (
    SELECT 1
    FROM glo."GloAppointmentAssignmentEventType" existing
    WHERE existing."EventTypeId" = v."EventTypeId"
);
