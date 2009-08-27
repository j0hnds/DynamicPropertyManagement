CREATE TABLE  `online_logging`.`DYN_DATA_TYPE` (
  `DYN_DATA_TYPE_ID` int(11) NOT NULL COMMENT 'The unique identifier for the data type',
  `DESCRIPTION` varchar(10) NOT NULL COMMENT 'The name of the data type',
  PRIMARY KEY  (`DYN_DATA_TYPE_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1
