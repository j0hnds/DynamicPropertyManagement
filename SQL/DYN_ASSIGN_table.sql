CREATE TABLE  `online_logging`.`DYN_ASSIGN` (
  `DYN_ASSIGN_ID` int(11) NOT NULL auto_increment COMMENT 'The unique identifier for the row.',
  `APPLICATION_ID` int(11) NOT NULL COMMENT 'The application',
  `DYN_PROPERTY_ID` int(11) NOT NULL COMMENT 'The foreign key to the dynamic property',
  `QUALIFIER` varchar(20) default NULL COMMENT 'A qualifier for the property.',
  `DFLT_VALUE` varchar(255) default NULL COMMENT 'The default value of the property',
  `MOD_DT` timestamp NOT NULL default CURRENT_TIMESTAMP COMMENT 'The last modification time for the record',
  PRIMARY KEY  (`DYN_ASSIGN_ID`),
  KEY `fk_property_constraint` (`DYN_PROPERTY_ID`),
  KEY `fk_app_constraint` (`APPLICATION_ID`),
  CONSTRAINT `fk_app_constraint` FOREIGN KEY (`APPLICATION_ID`) REFERENCES `DYN_APPLICATION` (`DYN_APPLICATION_ID`),
  CONSTRAINT `fk_property_constraint` FOREIGN KEY (`DYN_PROPERTY_ID`) REFERENCES `DYN_PROPERTY` (`DYN_PROPERTY_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1 COMMENT='Assigns a default value to a property'
