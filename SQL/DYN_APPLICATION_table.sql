CREATE TABLE  `online_logging`.`DYN_APPLICATION` (
  `DYN_APPLICATION_ID` int(11) NOT NULL auto_increment COMMENT 'Unique identifier',
  `APPLICATION_NAME` varchar(20) NOT NULL COMMENT 'Name of the application',
  PRIMARY KEY  (`DYN_APPLICATION_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1 COMMENT='Applications'
