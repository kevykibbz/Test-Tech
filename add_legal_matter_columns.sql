-- SQL Script to add missing columns to the legal matter table
-- Execute this script in Azure Data Studio

-- First, let's check what columns already exist in the Matter table
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Matter'
ORDER BY ORDINAL_POSITION;

-- Add missing columns to the Matter table (only if they don't exist)

-- Add Contract Type column (only if it doesn't exist)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Matter' AND COLUMN_NAME = 'ContractType')
BEGIN
    ALTER TABLE Matter ADD ContractType NVARCHAR(255) NULL;
    PRINT 'Added ContractType column';
END
ELSE
    PRINT 'ContractType column already exists';

-- Add Status column (only if it doesn't exist)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Matter' AND COLUMN_NAME = 'Status')
BEGIN
    ALTER TABLE Matter ADD Status NVARCHAR(100) NULL;
    PRINT 'Added Status column';
END
ELSE
    PRINT 'Status column already exists';

-- Add Contract Value column (only if it doesn't exist)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Matter' AND COLUMN_NAME = 'ContractValue')
BEGIN
    ALTER TABLE Matter ADD ContractValue DECIMAL(18,2) NULL;
    PRINT 'Added ContractValue column';
END
ELSE
    PRINT 'ContractValue column already exists';

-- Add Governing Law column (only if it doesn't exist)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Matter' AND COLUMN_NAME = 'GoverningLaw')
BEGIN
    ALTER TABLE Matter ADD GoverningLaw NVARCHAR(255) NULL;
    PRINT 'Added GoverningLaw column';
END
ELSE
    PRINT 'GoverningLaw column already exists';

-- Add Description column (only if it doesn't exist)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Matter' AND COLUMN_NAME = 'Description')
BEGIN
    ALTER TABLE Matter ADD Description NVARCHAR(MAX) NULL;
    PRINT 'Added Description column';
END
ELSE
    PRINT 'Description column already exists';

-- Add Effective Date column (only if it doesn't exist)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Matter' AND COLUMN_NAME = 'EffectiveDate')
BEGIN
    ALTER TABLE Matter ADD EffectiveDate DATETIME2 NULL;
    PRINT 'Added EffectiveDate column';
END
ELSE
    PRINT 'EffectiveDate column already exists';

-- Add Expiration Date column (only if it doesn't exist)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Matter' AND COLUMN_NAME = 'ExpirationDate')
BEGIN
    ALTER TABLE Matter ADD ExpirationDate DATETIME2 NULL;
    PRINT 'Added ExpirationDate column';
END
ELSE
    PRINT 'ExpirationDate column already exists';

-- Add Parties column (only if it doesn't exist)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Matter' AND COLUMN_NAME = 'Parties')
BEGIN
    ALTER TABLE Matter ADD Parties NVARCHAR(MAX) NULL;
    PRINT 'Added Parties column';
END
ELSE
    PRINT 'Parties column already exists';

-- Verify the new columns were added
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Matter'
ORDER BY ORDINAL_POSITION;

-- Verify the new columns were added
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Matter'
ORDER BY ORDINAL_POSITION;

-- Optional: Add some sample data to test the new columns
-- UPDATE Matter 
-- SET 
--     ContractType = 'Service Agreement',
--     Status = 'Active',
--     ContractValue = 100000.00,
--     GoverningLaw = 'New York State Law',
--     Description = 'Sample legal matter description',
--     EffectiveDate = GETDATE(),
--     ExpirationDate = DATEADD(YEAR, 1, GETDATE()),
--     Parties = '["Party A", "Party B"]'
-- WHERE Id = 'your-legal-matter-id-here';
