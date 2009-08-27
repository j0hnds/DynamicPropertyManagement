CREATE TABLE  `online_logging`.`DYN_EFFECTIVE` (
  `DYN_EFFECTIVE_ID` int(11) NOT NULL auto_increment COMMENT 'The unique identifier for the row',
  `DYN_ASSIGN_ID` int(11) NOT NULL COMMENT 'Foreign key to the assignment table.',
  `EFF_START_DT` datetime default NULL COMMENT 'The effective start date for the assignment',
  `EFF_END_DT` datetime default NULL COMMENT 'The effective end date for the assignment',
  `MOD_DT` timestamp NOT NULL default CURRENT_TIMESTAMP COMMENT 'The last modification date/time for the row.',
  PRIMARY KEY  (`DYN_EFFECTIVE_ID`),
  KEY `assign_fk_constraint` (`DYN_ASSIGN_ID`),
  CONSTRAINT `assign_fk_constraint` FOREIGN KEY (`DYN_ASSIGN_ID`) REFERENCES `DYN_ASSIGN` (`DYN_ASSIGN_ID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1 COMMENT='Defines the effective dates for a property'
