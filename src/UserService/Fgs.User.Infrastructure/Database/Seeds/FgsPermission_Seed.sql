-- FgsPermission_Seed.sql
-- Idempotent seed for identity.FgsPermission master catalog.
-- Run after migration 20260713180610_AddIdentityAuthorizationAndApiEntities.

BEGIN;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('USER.VIEW','Identity','User','View','View User','Allows users to view User records.',1,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('USER.CREATE','Identity','User','Create','Create User','Allows users to create User records.',2,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('USER.EDIT','Identity','User','Edit','Edit User','Allows users to edit User records.',3,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('USER.DELETE','Identity','User','Delete','Delete User','Allows users to delete User records.',4,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ROLE.VIEW','Identity','Role','View','View Role','Allows users to view Role records.',5,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ROLE.CREATE','Identity','Role','Create','Create Role','Allows users to create Role records.',6,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ROLE.EDIT','Identity','Role','Edit','Edit Role','Allows users to edit Role records.',7,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ROLE.DELETE','Identity','Role','Delete','Delete Role','Allows users to delete Role records.',8,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PERMISSION.VIEW','Identity','Permission','View','View Permission','Allows users to view Permission records.',9,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PERMISSION.CREATE','Identity','Permission','Create','Create Permission','Allows users to create Permission records.',10,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PERMISSION.EDIT','Identity','Permission','Edit','Edit Permission','Allows users to edit Permission records.',11,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PERMISSION.DELETE','Identity','Permission','Delete','Delete Permission','Allows users to delete Permission records.',12,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('APICLIENT.VIEW','Identity','ApiClient','View','View ApiClient','Allows users to view ApiClient records.',13,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('APICLIENT.CREATE','Identity','ApiClient','Create','Create ApiClient','Allows users to create ApiClient records.',14,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('APICLIENT.EDIT','Identity','ApiClient','Edit','Edit ApiClient','Allows users to edit ApiClient records.',15,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('APICLIENT.DELETE','Identity','ApiClient','Delete','Delete ApiClient','Allows users to delete ApiClient records.',16,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('APIKEY.VIEW','Identity','ApiKey','View','View ApiKey','Allows users to view ApiKey records.',17,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('APIKEY.CREATE','Identity','ApiKey','Create','Create ApiKey','Allows users to create ApiKey records.',18,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('APIKEY.EDIT','Identity','ApiKey','Edit','Edit ApiKey','Allows users to edit ApiKey records.',19,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('APIKEY.DELETE','Identity','ApiKey','Delete','Delete ApiKey','Allows users to delete ApiKey records.',20,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WEBHOOK.VIEW','Identity','Webhook','View','View Webhook','Allows users to view Webhook records.',21,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WEBHOOK.CREATE','Identity','Webhook','Create','Create Webhook','Allows users to create Webhook records.',22,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WEBHOOK.EDIT','Identity','Webhook','Edit','Edit Webhook','Allows users to edit Webhook records.',23,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WEBHOOK.DELETE','Identity','Webhook','Delete','Delete Webhook','Allows users to delete Webhook records.',24,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('AUDIT.VIEW','Identity','Audit','View','View Audit','Allows users to view Audit records.',25,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('AUDIT.CREATE','Identity','Audit','Create','Create Audit','Allows users to create Audit records.',26,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('AUDIT.EDIT','Identity','Audit','Edit','Edit Audit','Allows users to edit Audit records.',27,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('AUDIT.DELETE','Identity','Audit','Delete','Delete Audit','Allows users to delete Audit records.',28,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CUSTOMER.VIEW','CRM','Customer','View','View Customer','Allows users to view Customer records.',29,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CUSTOMER.CREATE','CRM','Customer','Create','Create Customer','Allows users to create Customer records.',30,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CUSTOMER.EDIT','CRM','Customer','Edit','Edit Customer','Allows users to edit Customer records.',31,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CUSTOMER.DELETE','CRM','Customer','Delete','Delete Customer','Allows users to delete Customer records.',32,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTACT.VIEW','CRM','Contact','View','View Contact','Allows users to view Contact records.',33,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTACT.CREATE','CRM','Contact','Create','Create Contact','Allows users to create Contact records.',34,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTACT.EDIT','CRM','Contact','Edit','Edit Contact','Allows users to edit Contact records.',35,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTACT.DELETE','CRM','Contact','Delete','Delete Contact','Allows users to delete Contact records.',36,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LOCATION.VIEW','CRM','Location','View','View Location','Allows users to view Location records.',37,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LOCATION.CREATE','CRM','Location','Create','Create Location','Allows users to create Location records.',38,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LOCATION.EDIT','CRM','Location','Edit','Edit Location','Allows users to edit Location records.',39,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LOCATION.DELETE','CRM','Location','Delete','Delete Location','Allows users to delete Location records.',40,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LEAD.VIEW','CRM','Lead','View','View Lead','Allows users to view Lead records.',41,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LEAD.CREATE','CRM','Lead','Create','Create Lead','Allows users to create Lead records.',42,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LEAD.EDIT','CRM','Lead','Edit','Edit Lead','Allows users to edit Lead records.',43,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LEAD.DELETE','CRM','Lead','Delete','Delete Lead','Allows users to delete Lead records.',44,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('OPPORTUNITY.VIEW','CRM','Opportunity','View','View Opportunity','Allows users to view Opportunity records.',45,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('OPPORTUNITY.CREATE','CRM','Opportunity','Create','Create Opportunity','Allows users to create Opportunity records.',46,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('OPPORTUNITY.EDIT','CRM','Opportunity','Edit','Edit Opportunity','Allows users to edit Opportunity records.',47,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('OPPORTUNITY.DELETE','CRM','Opportunity','Delete','Delete Opportunity','Allows users to delete Opportunity records.',48,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WORKORDER.VIEW','Service Management','WorkOrder','View','View WorkOrder','Allows users to view WorkOrder records.',49,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WORKORDER.CREATE','Service Management','WorkOrder','Create','Create WorkOrder','Allows users to create WorkOrder records.',50,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WORKORDER.EDIT','Service Management','WorkOrder','Edit','Edit WorkOrder','Allows users to edit WorkOrder records.',51,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WORKORDER.DELETE','Service Management','WorkOrder','Delete','Delete WorkOrder','Allows users to delete WorkOrder records.',52,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PROJECT.VIEW','Service Management','Project','View','View Project','Allows users to view Project records.',53,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PROJECT.CREATE','Service Management','Project','Create','Create Project','Allows users to create Project records.',54,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PROJECT.EDIT','Service Management','Project','Edit','Edit Project','Allows users to edit Project records.',55,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PROJECT.DELETE','Service Management','Project','Delete','Delete Project','Allows users to delete Project records.',56,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SCHEDULE.VIEW','Service Management','Schedule','View','View Schedule','Allows users to view Schedule records.',57,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SCHEDULE.CREATE','Service Management','Schedule','Create','Create Schedule','Allows users to create Schedule records.',58,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SCHEDULE.EDIT','Service Management','Schedule','Edit','Edit Schedule','Allows users to edit Schedule records.',59,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SCHEDULE.DELETE','Service Management','Schedule','Delete','Delete Schedule','Allows users to delete Schedule records.',60,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DISPATCH.VIEW','Service Management','Dispatch','View','View Dispatch','Allows users to view Dispatch records.',61,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DISPATCH.CREATE','Service Management','Dispatch','Create','Create Dispatch','Allows users to create Dispatch records.',62,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DISPATCH.EDIT','Service Management','Dispatch','Edit','Edit Dispatch','Allows users to edit Dispatch records.',63,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DISPATCH.DELETE','Service Management','Dispatch','Delete','Delete Dispatch','Allows users to delete Dispatch records.',64,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TECHNICIAN.VIEW','Service Management','Technician','View','View Technician','Allows users to view Technician records.',65,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TECHNICIAN.CREATE','Service Management','Technician','Create','Create Technician','Allows users to create Technician records.',66,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TECHNICIAN.EDIT','Service Management','Technician','Edit','Edit Technician','Allows users to edit Technician records.',67,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TECHNICIAN.DELETE','Service Management','Technician','Delete','Delete Technician','Allows users to delete Technician records.',68,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TIMEENTRY.VIEW','Service Management','TimeEntry','View','View TimeEntry','Allows users to view TimeEntry records.',69,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TIMEENTRY.CREATE','Service Management','TimeEntry','Create','Create TimeEntry','Allows users to create TimeEntry records.',70,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TIMEENTRY.EDIT','Service Management','TimeEntry','Edit','Edit TimeEntry','Allows users to edit TimeEntry records.',71,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TIMEENTRY.DELETE','Service Management','TimeEntry','Delete','Delete TimeEntry','Allows users to delete TimeEntry records.',72,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CHECKLIST.VIEW','Service Management','Checklist','View','View Checklist','Allows users to view Checklist records.',73,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CHECKLIST.CREATE','Service Management','Checklist','Create','Create Checklist','Allows users to create Checklist records.',74,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CHECKLIST.EDIT','Service Management','Checklist','Edit','Edit Checklist','Allows users to edit Checklist records.',75,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CHECKLIST.DELETE','Service Management','Checklist','Delete','Delete Checklist','Allows users to delete Checklist records.',76,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ESTIMATE.VIEW','Sales','Estimate','View','View Estimate','Allows users to view Estimate records.',77,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ESTIMATE.CREATE','Sales','Estimate','Create','Create Estimate','Allows users to create Estimate records.',78,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ESTIMATE.EDIT','Sales','Estimate','Edit','Edit Estimate','Allows users to edit Estimate records.',79,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ESTIMATE.DELETE','Sales','Estimate','Delete','Delete Estimate','Allows users to delete Estimate records.',80,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ESTIMATETEMPLATE.VIEW','Sales','EstimateTemplate','View','View EstimateTemplate','Allows users to view EstimateTemplate records.',81,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ESTIMATETEMPLATE.CREATE','Sales','EstimateTemplate','Create','Create EstimateTemplate','Allows users to create EstimateTemplate records.',82,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ESTIMATETEMPLATE.EDIT','Sales','EstimateTemplate','Edit','Edit EstimateTemplate','Allows users to edit EstimateTemplate records.',83,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ESTIMATETEMPLATE.DELETE','Sales','EstimateTemplate','Delete','Delete EstimateTemplate','Allows users to delete EstimateTemplate records.',84,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PRICEBOOK.VIEW','Sales','PriceBook','View','View PriceBook','Allows users to view PriceBook records.',85,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PRICEBOOK.CREATE','Sales','PriceBook','Create','Create PriceBook','Allows users to create PriceBook records.',86,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PRICEBOOK.EDIT','Sales','PriceBook','Edit','Edit PriceBook','Allows users to edit PriceBook records.',87,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PRICEBOOK.DELETE','Sales','PriceBook','Delete','Delete PriceBook','Allows users to delete PriceBook records.',88,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVOICE.VIEW','Billing','Invoice','View','View Invoice','Allows users to view Invoice records.',89,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVOICE.CREATE','Billing','Invoice','Create','Create Invoice','Allows users to create Invoice records.',90,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVOICE.EDIT','Billing','Invoice','Edit','Edit Invoice','Allows users to edit Invoice records.',91,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVOICE.DELETE','Billing','Invoice','Delete','Delete Invoice','Allows users to delete Invoice records.',92,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENT.VIEW','Billing','Payment','View','View Payment','Allows users to view Payment records.',93,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENT.CREATE','Billing','Payment','Create','Create Payment','Allows users to create Payment records.',94,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENT.EDIT','Billing','Payment','Edit','Edit Payment','Allows users to edit Payment records.',95,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENT.DELETE','Billing','Payment','Delete','Delete Payment','Allows users to delete Payment records.',96,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REFUND.VIEW','Billing','Refund','View','View Refund','Allows users to view Refund records.',97,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REFUND.CREATE','Billing','Refund','Create','Create Refund','Allows users to create Refund records.',98,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REFUND.EDIT','Billing','Refund','Edit','Edit Refund','Allows users to edit Refund records.',99,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REFUND.DELETE','Billing','Refund','Delete','Delete Refund','Allows users to delete Refund records.',100,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTRACT.VIEW','Contracts','Contract','View','View Contract','Allows users to view Contract records.',101,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTRACT.CREATE','Contracts','Contract','Create','Create Contract','Allows users to create Contract records.',102,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTRACT.EDIT','Contracts','Contract','Edit','Edit Contract','Allows users to edit Contract records.',103,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTRACT.DELETE','Contracts','Contract','Delete','Delete Contract','Allows users to delete Contract records.',104,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTRACTTEMPLATE.VIEW','Contracts','ContractTemplate','View','View ContractTemplate','Allows users to view ContractTemplate records.',105,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTRACTTEMPLATE.CREATE','Contracts','ContractTemplate','Create','Create ContractTemplate','Allows users to create ContractTemplate records.',106,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTRACTTEMPLATE.EDIT','Contracts','ContractTemplate','Edit','Edit ContractTemplate','Allows users to edit ContractTemplate records.',107,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('CONTRACTTEMPLATE.DELETE','Contracts','ContractTemplate','Delete','Delete ContractTemplate','Allows users to delete ContractTemplate records.',108,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WARRANTY.VIEW','Contracts','Warranty','View','View Warranty','Allows users to view Warranty records.',109,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WARRANTY.CREATE','Contracts','Warranty','Create','Create Warranty','Allows users to create Warranty records.',110,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WARRANTY.EDIT','Contracts','Warranty','Edit','Edit Warranty','Allows users to edit Warranty records.',111,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WARRANTY.DELETE','Contracts','Warranty','Delete','Delete Warranty','Allows users to delete Warranty records.',112,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICEAGREEMENT.VIEW','Contracts','ServiceAgreement','View','View ServiceAgreement','Allows users to view ServiceAgreement records.',113,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICEAGREEMENT.CREATE','Contracts','ServiceAgreement','Create','Create ServiceAgreement','Allows users to create ServiceAgreement records.',114,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICEAGREEMENT.EDIT','Contracts','ServiceAgreement','Edit','Edit ServiceAgreement','Allows users to edit ServiceAgreement records.',115,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICEAGREEMENT.DELETE','Contracts','ServiceAgreement','Delete','Delete ServiceAgreement','Allows users to delete ServiceAgreement records.',116,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYITEM.VIEW','Inventory','InventoryItem','View','View InventoryItem','Allows users to view InventoryItem records.',117,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYITEM.CREATE','Inventory','InventoryItem','Create','Create InventoryItem','Allows users to create InventoryItem records.',118,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYITEM.EDIT','Inventory','InventoryItem','Edit','Edit InventoryItem','Allows users to edit InventoryItem records.',119,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYITEM.DELETE','Inventory','InventoryItem','Delete','Delete InventoryItem','Allows users to delete InventoryItem records.',120,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WAREHOUSE.VIEW','Inventory','Warehouse','View','View Warehouse','Allows users to view Warehouse records.',121,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WAREHOUSE.CREATE','Inventory','Warehouse','Create','Create Warehouse','Allows users to create Warehouse records.',122,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WAREHOUSE.EDIT','Inventory','Warehouse','Edit','Edit Warehouse','Allows users to edit Warehouse records.',123,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WAREHOUSE.DELETE','Inventory','Warehouse','Delete','Delete Warehouse','Allows users to delete Warehouse records.',124,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYADJUSTMENT.VIEW','Inventory','InventoryAdjustment','View','View InventoryAdjustment','Allows users to view InventoryAdjustment records.',125,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYADJUSTMENT.CREATE','Inventory','InventoryAdjustment','Create','Create InventoryAdjustment','Allows users to create InventoryAdjustment records.',126,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYADJUSTMENT.EDIT','Inventory','InventoryAdjustment','Edit','Edit InventoryAdjustment','Allows users to edit InventoryAdjustment records.',127,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYADJUSTMENT.DELETE','Inventory','InventoryAdjustment','Delete','Delete InventoryAdjustment','Allows users to delete InventoryAdjustment records.',128,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PURCHASEORDER.VIEW','Inventory','PurchaseOrder','View','View PurchaseOrder','Allows users to view PurchaseOrder records.',129,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PURCHASEORDER.CREATE','Inventory','PurchaseOrder','Create','Create PurchaseOrder','Allows users to create PurchaseOrder records.',130,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PURCHASEORDER.EDIT','Inventory','PurchaseOrder','Edit','Edit PurchaseOrder','Allows users to edit PurchaseOrder records.',131,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PURCHASEORDER.DELETE','Inventory','PurchaseOrder','Delete','Delete PurchaseOrder','Allows users to delete PurchaseOrder records.',132,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PURCHASEORDERRECEIPT.VIEW','Inventory','PurchaseOrderReceipt','View','View PurchaseOrderReceipt','Allows users to view PurchaseOrderReceipt records.',133,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PURCHASEORDERRECEIPT.CREATE','Inventory','PurchaseOrderReceipt','Create','Create PurchaseOrderReceipt','Allows users to create PurchaseOrderReceipt records.',134,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PURCHASEORDERRECEIPT.EDIT','Inventory','PurchaseOrderReceipt','Edit','Edit PurchaseOrderReceipt','Allows users to edit PurchaseOrderReceipt records.',135,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PURCHASEORDERRECEIPT.DELETE','Inventory','PurchaseOrderReceipt','Delete','Delete PurchaseOrderReceipt','Allows users to delete PurchaseOrderReceipt records.',136,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYTRANSFER.VIEW','Inventory','InventoryTransfer','View','View InventoryTransfer','Allows users to view InventoryTransfer records.',137,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYTRANSFER.CREATE','Inventory','InventoryTransfer','Create','Create InventoryTransfer','Allows users to create InventoryTransfer records.',138,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYTRANSFER.EDIT','Inventory','InventoryTransfer','Edit','Edit InventoryTransfer','Allows users to edit InventoryTransfer records.',139,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INVENTORYTRANSFER.DELETE','Inventory','InventoryTransfer','Delete','Delete InventoryTransfer','Allows users to delete InventoryTransfer records.',140,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('VENDOR.VIEW','Inventory','Vendor','View','View Vendor','Allows users to view Vendor records.',141,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('VENDOR.CREATE','Inventory','Vendor','Create','Create Vendor','Allows users to create Vendor records.',142,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('VENDOR.EDIT','Inventory','Vendor','Edit','Edit Vendor','Allows users to edit Vendor records.',143,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('VENDOR.DELETE','Inventory','Vendor','Delete','Delete Vendor','Allows users to delete Vendor records.',144,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ASSET.VIEW','Assets','Asset','View','View Asset','Allows users to view Asset records.',145,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ASSET.CREATE','Assets','Asset','Create','Create Asset','Allows users to create Asset records.',146,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ASSET.EDIT','Assets','Asset','Edit','Edit Asset','Allows users to edit Asset records.',147,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ASSET.DELETE','Assets','Asset','Delete','Delete Asset','Allows users to delete Asset records.',148,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EQUIPMENT.VIEW','Assets','Equipment','View','View Equipment','Allows users to view Equipment records.',149,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EQUIPMENT.CREATE','Assets','Equipment','Create','Create Equipment','Allows users to create Equipment records.',150,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EQUIPMENT.EDIT','Assets','Equipment','Edit','Edit Equipment','Allows users to edit Equipment records.',151,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EQUIPMENT.DELETE','Assets','Equipment','Delete','Delete Equipment','Allows users to delete Equipment records.',152,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EQUIPMENTREADING.VIEW','Assets','EquipmentReading','View','View EquipmentReading','Allows users to view EquipmentReading records.',153,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EQUIPMENTREADING.CREATE','Assets','EquipmentReading','Create','Create EquipmentReading','Allows users to create EquipmentReading records.',154,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EQUIPMENTREADING.EDIT','Assets','EquipmentReading','Edit','Edit EquipmentReading','Allows users to edit EquipmentReading records.',155,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EQUIPMENTREADING.DELETE','Assets','EquipmentReading','Delete','Delete EquipmentReading','Allows users to delete EquipmentReading records.',156,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('MAINTENANCEPLAN.VIEW','Assets','MaintenancePlan','View','View MaintenancePlan','Allows users to view MaintenancePlan records.',157,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('MAINTENANCEPLAN.CREATE','Assets','MaintenancePlan','Create','Create MaintenancePlan','Allows users to create MaintenancePlan records.',158,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('MAINTENANCEPLAN.EDIT','Assets','MaintenancePlan','Edit','Edit MaintenancePlan','Allows users to edit MaintenancePlan records.',159,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('MAINTENANCEPLAN.DELETE','Assets','MaintenancePlan','Delete','Delete MaintenancePlan','Allows users to delete MaintenancePlan records.',160,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('MAINTENANCESCHEDULE.VIEW','Assets','MaintenanceSchedule','View','View MaintenanceSchedule','Allows users to view MaintenanceSchedule records.',161,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('MAINTENANCESCHEDULE.CREATE','Assets','MaintenanceSchedule','Create','Create MaintenanceSchedule','Allows users to create MaintenanceSchedule records.',162,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('MAINTENANCESCHEDULE.EDIT','Assets','MaintenanceSchedule','Edit','Edit MaintenanceSchedule','Allows users to edit MaintenanceSchedule records.',163,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('MAINTENANCESCHEDULE.DELETE','Assets','MaintenanceSchedule','Delete','Delete MaintenanceSchedule','Allows users to delete MaintenanceSchedule records.',164,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EMAILTEMPLATE.VIEW','Notifications','EmailTemplate','View','View EmailTemplate','Allows users to view EmailTemplate records.',165,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EMAILTEMPLATE.CREATE','Notifications','EmailTemplate','Create','Create EmailTemplate','Allows users to create EmailTemplate records.',166,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EMAILTEMPLATE.EDIT','Notifications','EmailTemplate','Edit','Edit EmailTemplate','Allows users to edit EmailTemplate records.',167,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EMAILTEMPLATE.DELETE','Notifications','EmailTemplate','Delete','Delete EmailTemplate','Allows users to delete EmailTemplate records.',168,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SMSTEMPLATE.VIEW','Notifications','SmsTemplate','View','View SmsTemplate','Allows users to view SmsTemplate records.',169,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SMSTEMPLATE.CREATE','Notifications','SmsTemplate','Create','Create SmsTemplate','Allows users to create SmsTemplate records.',170,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SMSTEMPLATE.EDIT','Notifications','SmsTemplate','Edit','Edit SmsTemplate','Allows users to edit SmsTemplate records.',171,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SMSTEMPLATE.DELETE','Notifications','SmsTemplate','Delete','Delete SmsTemplate','Allows users to delete SmsTemplate records.',172,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EMAILHISTORY.VIEW','Notifications','EmailHistory','View','View EmailHistory','Allows users to view EmailHistory records.',173,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EMAILHISTORY.CREATE','Notifications','EmailHistory','Create','Create EmailHistory','Allows users to create EmailHistory records.',174,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EMAILHISTORY.EDIT','Notifications','EmailHistory','Edit','Edit EmailHistory','Allows users to edit EmailHistory records.',175,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('EMAILHISTORY.DELETE','Notifications','EmailHistory','Delete','Delete EmailHistory','Allows users to delete EmailHistory records.',176,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SMSHISTORY.VIEW','Notifications','SmsHistory','View','View SmsHistory','Allows users to view SmsHistory records.',177,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SMSHISTORY.CREATE','Notifications','SmsHistory','Create','Create SmsHistory','Allows users to create SmsHistory records.',178,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SMSHISTORY.EDIT','Notifications','SmsHistory','Edit','Edit SmsHistory','Allows users to edit SmsHistory records.',179,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SMSHISTORY.DELETE','Notifications','SmsHistory','Delete','Delete SmsHistory','Allows users to delete SmsHistory records.',180,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REMINDER.VIEW','Notifications','Reminder','View','View Reminder','Allows users to view Reminder records.',181,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REMINDER.CREATE','Notifications','Reminder','Create','Create Reminder','Allows users to create Reminder records.',182,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REMINDER.EDIT','Notifications','Reminder','Edit','Edit Reminder','Allows users to edit Reminder records.',183,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REMINDER.DELETE','Notifications','Reminder','Delete','Delete Reminder','Allows users to delete Reminder records.',184,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DOCUMENT.VIEW','Files','Document','View','View Document','Allows users to view Document records.',185,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DOCUMENT.CREATE','Files','Document','Create','Create Document','Allows users to create Document records.',186,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DOCUMENT.EDIT','Files','Document','Edit','Edit Document','Allows users to edit Document records.',187,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DOCUMENT.DELETE','Files','Document','Delete','Delete Document','Allows users to delete Document records.',188,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ATTACHMENT.VIEW','Files','Attachment','View','View Attachment','Allows users to view Attachment records.',189,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ATTACHMENT.CREATE','Files','Attachment','Create','Create Attachment','Allows users to create Attachment records.',190,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ATTACHMENT.EDIT','Files','Attachment','Edit','Edit Attachment','Allows users to edit Attachment records.',191,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('ATTACHMENT.DELETE','Files','Attachment','Delete','Delete Attachment','Allows users to delete Attachment records.',192,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('IMAGE.VIEW','Files','Image','View','View Image','Allows users to view Image records.',193,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('IMAGE.CREATE','Files','Image','Create','Create Image','Allows users to create Image records.',194,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('IMAGE.EDIT','Files','Image','Edit','Edit Image','Allows users to edit Image records.',195,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('IMAGE.DELETE','Files','Image','Delete','Delete Image','Allows users to delete Image records.',196,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DASHBOARD.VIEW','Analytics','Dashboard','View','View Dashboard','Allows users to view Dashboard records.',197,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DASHBOARD.CREATE','Analytics','Dashboard','Create','Create Dashboard','Allows users to create Dashboard records.',198,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DASHBOARD.EDIT','Analytics','Dashboard','Edit','Edit Dashboard','Allows users to edit Dashboard records.',199,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DASHBOARD.DELETE','Analytics','Dashboard','Delete','Delete Dashboard','Allows users to delete Dashboard records.',200,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REPORT.VIEW','Analytics','Report','View','View Report','Allows users to view Report records.',201,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REPORT.CREATE','Analytics','Report','Create','Create Report','Allows users to create Report records.',202,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REPORT.EDIT','Analytics','Report','Edit','Edit Report','Allows users to edit Report records.',203,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('REPORT.DELETE','Analytics','Report','Delete','Delete Report','Allows users to delete Report records.',204,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WIDGET.VIEW','Analytics','Widget','View','View Widget','Allows users to view Widget records.',205,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WIDGET.CREATE','Analytics','Widget','Create','Create Widget','Allows users to create Widget records.',206,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WIDGET.EDIT','Analytics','Widget','Edit','Edit Widget','Allows users to edit Widget records.',207,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WIDGET.DELETE','Analytics','Widget','Delete','Delete Widget','Allows users to delete Widget records.',208,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('COMPANY.VIEW','Administration','Company','View','View Company','Allows users to view Company records.',209,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('COMPANY.CREATE','Administration','Company','Create','Create Company','Allows users to create Company records.',210,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('COMPANY.EDIT','Administration','Company','Edit','Edit Company','Allows users to edit Company records.',211,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('COMPANY.DELETE','Administration','Company','Delete','Delete Company','Allows users to delete Company records.',212,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DEPARTMENT.VIEW','Administration','Department','View','View Department','Allows users to view Department records.',213,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DEPARTMENT.CREATE','Administration','Department','Create','Create Department','Allows users to create Department records.',214,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DEPARTMENT.EDIT','Administration','Department','Edit','Edit Department','Allows users to edit Department records.',215,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('DEPARTMENT.DELETE','Administration','Department','Delete','Delete Department','Allows users to delete Department records.',216,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSHOUR.VIEW','Administration','BusinessHour','View','View BusinessHour','Allows users to view BusinessHour records.',217,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSHOUR.CREATE','Administration','BusinessHour','Create','Create BusinessHour','Allows users to create BusinessHour records.',218,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSHOUR.EDIT','Administration','BusinessHour','Edit','Edit BusinessHour','Allows users to edit BusinessHour records.',219,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSHOUR.DELETE','Administration','BusinessHour','Delete','Delete BusinessHour','Allows users to delete BusinessHour records.',220,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('HOLIDAY.VIEW','Administration','Holiday','View','View Holiday','Allows users to view Holiday records.',221,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('HOLIDAY.CREATE','Administration','Holiday','Create','Create Holiday','Allows users to create Holiday records.',222,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('HOLIDAY.EDIT','Administration','Holiday','Edit','Edit Holiday','Allows users to edit Holiday records.',223,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('HOLIDAY.DELETE','Administration','Holiday','Delete','Delete Holiday','Allows users to delete Holiday records.',224,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SETUP.VIEW','Setup','Setup','View','View Setup','Allows users to view Setup records.',225,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SETUP.CREATE','Setup','Setup','Create','Create Setup','Allows users to create Setup records.',226,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SETUP.EDIT','Setup','Setup','Edit','Edit Setup','Allows users to edit Setup records.',227,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SETUP.DELETE','Setup','Setup','Delete','Delete Setup','Allows users to delete Setup records.',228,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSUNIT.VIEW','Setup','BusinessUnit','View','View BusinessUnit','Allows users to view BusinessUnit records.',229,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSUNIT.CREATE','Setup','BusinessUnit','Create','Create BusinessUnit','Allows users to create BusinessUnit records.',230,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSUNIT.EDIT','Setup','BusinessUnit','Edit','Edit BusinessUnit','Allows users to edit BusinessUnit records.',231,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSUNIT.DELETE','Setup','BusinessUnit','Delete','Delete BusinessUnit','Allows users to delete BusinessUnit records.',232,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSTYPE.VIEW','Setup','BusinessType','View','View BusinessType','Allows users to view BusinessType records.',233,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSTYPE.CREATE','Setup','BusinessType','Create','Create BusinessType','Allows users to create BusinessType records.',234,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSTYPE.EDIT','Setup','BusinessType','Edit','Edit BusinessType','Allows users to edit BusinessType records.',235,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('BUSINESSTYPE.DELETE','Setup','BusinessType','Delete','Delete BusinessType','Allows users to delete BusinessType records.',236,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('JOBTYPE.VIEW','Setup','JobType','View','View JobType','Allows users to view JobType records.',237,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('JOBTYPE.CREATE','Setup','JobType','Create','Create JobType','Allows users to create JobType records.',238,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('JOBTYPE.EDIT','Setup','JobType','Edit','Edit JobType','Allows users to edit JobType records.',239,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('JOBTYPE.DELETE','Setup','JobType','Delete','Delete JobType','Allows users to delete JobType records.',240,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WORKORDERTYPE.VIEW','Setup','WorkOrderType','View','View WorkOrderType','Allows users to view WorkOrderType records.',241,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WORKORDERTYPE.CREATE','Setup','WorkOrderType','Create','Create WorkOrderType','Allows users to create WorkOrderType records.',242,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WORKORDERTYPE.EDIT','Setup','WorkOrderType','Edit','Edit WorkOrderType','Allows users to edit WorkOrderType records.',243,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('WORKORDERTYPE.DELETE','Setup','WorkOrderType','Delete','Delete WorkOrderType','Allows users to delete WorkOrderType records.',244,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICETYPE.VIEW','Setup','ServiceType','View','View ServiceType','Allows users to view ServiceType records.',245,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICETYPE.CREATE','Setup','ServiceType','Create','Create ServiceType','Allows users to create ServiceType records.',246,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICETYPE.EDIT','Setup','ServiceType','Edit','Edit ServiceType','Allows users to edit ServiceType records.',247,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICETYPE.DELETE','Setup','ServiceType','Delete','Delete ServiceType','Allows users to delete ServiceType records.',248,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PRIORITY.VIEW','Setup','Priority','View','View Priority','Allows users to view Priority records.',249,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PRIORITY.CREATE','Setup','Priority','Create','Create Priority','Allows users to create Priority records.',250,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PRIORITY.EDIT','Setup','Priority','Edit','Edit Priority','Allows users to edit Priority records.',251,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PRIORITY.DELETE','Setup','Priority','Delete','Delete Priority','Allows users to delete Priority records.',252,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('STATUS.VIEW','Setup','Status','View','View Status','Allows users to view Status records.',253,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('STATUS.CREATE','Setup','Status','Create','Create Status','Allows users to create Status records.',254,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('STATUS.EDIT','Setup','Status','Edit','Edit Status','Allows users to edit Status records.',255,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('STATUS.DELETE','Setup','Status','Delete','Delete Status','Allows users to delete Status records.',256,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENTTERM.VIEW','Setup','PaymentTerm','View','View PaymentTerm','Allows users to view PaymentTerm records.',257,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENTTERM.CREATE','Setup','PaymentTerm','Create','Create PaymentTerm','Allows users to create PaymentTerm records.',258,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENTTERM.EDIT','Setup','PaymentTerm','Edit','Edit PaymentTerm','Allows users to edit PaymentTerm records.',259,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENTTERM.DELETE','Setup','PaymentTerm','Delete','Delete PaymentTerm','Allows users to delete PaymentTerm records.',260,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TAXCODE.VIEW','Setup','TaxCode','View','View TaxCode','Allows users to view TaxCode records.',261,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TAXCODE.CREATE','Setup','TaxCode','Create','Create TaxCode','Allows users to create TaxCode records.',262,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TAXCODE.EDIT','Setup','TaxCode','Edit','Edit TaxCode','Allows users to edit TaxCode records.',263,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('TAXCODE.DELETE','Setup','TaxCode','Delete','Delete TaxCode','Allows users to delete TaxCode records.',264,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENTMETHOD.VIEW','Setup','PaymentMethod','View','View PaymentMethod','Allows users to view PaymentMethod records.',265,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENTMETHOD.CREATE','Setup','PaymentMethod','Create','Create PaymentMethod','Allows users to create PaymentMethod records.',266,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENTMETHOD.EDIT','Setup','PaymentMethod','Edit','Edit PaymentMethod','Allows users to edit PaymentMethod records.',267,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('PAYMENTMETHOD.DELETE','Setup','PaymentMethod','Delete','Delete PaymentMethod','Allows users to delete PaymentMethod records.',268,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LABORRATE.VIEW','Setup','LaborRate','View','View LaborRate','Allows users to view LaborRate records.',269,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LABORRATE.CREATE','Setup','LaborRate','Create','Create LaborRate','Allows users to create LaborRate records.',270,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LABORRATE.EDIT','Setup','LaborRate','Edit','Edit LaborRate','Allows users to edit LaborRate records.',271,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LABORRATE.DELETE','Setup','LaborRate','Delete','Delete LaborRate','Allows users to delete LaborRate records.',272,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICEAREA.VIEW','Setup','ServiceArea','View','View ServiceArea','Allows users to view ServiceArea records.',273,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICEAREA.CREATE','Setup','ServiceArea','Create','Create ServiceArea','Allows users to create ServiceArea records.',274,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICEAREA.EDIT','Setup','ServiceArea','Edit','Edit ServiceArea','Allows users to edit ServiceArea records.',275,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SERVICEAREA.DELETE','Setup','ServiceArea','Delete','Delete ServiceArea','Allows users to delete ServiceArea records.',276,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SETTING.VIEW','System','Setting','View','View Setting','Allows users to view Setting records.',277,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SETTING.CREATE','System','Setting','Create','Create Setting','Allows users to create Setting records.',278,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SETTING.EDIT','System','Setting','Edit','Edit Setting','Allows users to edit Setting records.',279,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('SETTING.DELETE','System','Setting','Delete','Delete Setting','Allows users to delete Setting records.',280,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('FEATURE.VIEW','System','Feature','View','View Feature','Allows users to view Feature records.',281,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('FEATURE.CREATE','System','Feature','Create','Create Feature','Allows users to create Feature records.',282,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('FEATURE.EDIT','System','Feature','Edit','Edit Feature','Allows users to edit Feature records.',283,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('FEATURE.DELETE','System','Feature','Delete','Delete Feature','Allows users to delete Feature records.',284,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INTEGRATION.VIEW','System','Integration','View','View Integration','Allows users to view Integration records.',285,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INTEGRATION.CREATE','System','Integration','Create','Create Integration','Allows users to create Integration records.',286,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INTEGRATION.EDIT','System','Integration','Edit','Edit Integration','Allows users to edit Integration records.',287,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('INTEGRATION.DELETE','System','Integration','Delete','Delete Integration','Allows users to delete Integration records.',288,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LICENSE.VIEW','System','License','View','View License','Allows users to view License records.',289,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LICENSE.CREATE','System','License','Create','Create License','Allows users to create License records.',290,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LICENSE.EDIT','System','License','Edit','Edit License','Allows users to edit License records.',291,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

INSERT INTO identity."FgsPermission"
("PermissionCode","Module","Resource","Action","Name","Description","DisplayOrder","IsActive","CreatedOn")
VALUES
('LICENSE.DELETE','System','License','Delete','Delete License','Allows users to delete License records.',292,true,now())
ON CONFLICT ("PermissionCode") DO NOTHING;

COMMIT;
