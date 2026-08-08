SELECT Name FROM AspNetRoles;
SELECT RoleId, ClaimType, ClaimValue FROM AspNetRoleClaims WHERE ClaimValue LIKE 'Permissions.Users%';
SELECT UserId, RoleId FROM AspNetUserRoles;
