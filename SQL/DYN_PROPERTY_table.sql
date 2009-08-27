CREATE TABLE  `online_logging`.`DYN_PROPERTY` (
  `DYN_PROPERTY_ID` int(11) NOT NULL auto_increment COMMENT 'The unique identifier for the property',
  `CATEGORY` varchar(20) NOT NULL COMMENT 'The name of the category for the property',
  `NAME` varchar(80) NOT NULL COMMENT 'The name of the property',
  `DATA_TYPE` int(11) NOT NULL COMMENT 'The data type of the property',
  `DESCRIPTION` varchar(80) NOT NULL COMMENT 'Description of the property',
  PRIMARY KEY  (`DYN_PROPERTY_ID`),
  KEY `new_fk_constraint` (`DATA_TYPE`),
  CONSTRAINT `new_fk_constraint` FOREIGN KEY (`DATA_TYPE`) REFERENCES `DYN_DATA_TYPE` (`DYN_DATA_TYPE_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=latin1 COMMENT='Defines the legal dynamic properties'
