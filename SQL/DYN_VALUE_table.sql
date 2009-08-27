CREATE TABLE  `online_logging`.`DYN_VALUE` (
  `DYN_VALUE_ID` int(11) NOT NULL auto_increment COMMENT 'Unique identifier for the row.',
  `DYN_EFFECTIVE_ID` int(11) NOT NULL COMMENT 'Foreign key to the effective row',
  `CRITERIA` varchar(80) default NULL COMMENT 'The criteria against which the current time is to be evaluated.',
  `PROP_VALUE` varchar(255) default NULL COMMENT 'The value to assign to the property',
  `CONTENT_VALUE` longtext COMMENT 'The content value',
  `MOD_DT` timestamp NOT NULL default CURRENT_TIMESTAMP COMMENT 'The last modification time for this row.',
  PRIMARY KEY  (`DYN_VALUE_ID`),
  KEY `eff_fk_constraint` (`DYN_EFFECTIVE_ID`),
  CONSTRAINT `eff_fk_constraint` FOREIGN KEY (`DYN_EFFECTIVE_ID`) REFERENCES `DYN_EFFECTIVE` (`DYN_EFFECTIVE_ID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1 COMMENT='Value assignment for dynamic property'
