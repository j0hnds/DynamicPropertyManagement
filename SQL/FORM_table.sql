CREATE TABLE  `online_logging`.`FORM` (
  `FORM_ID` int(11) NOT NULL COMMENT 'The numeric ID of the form',
  `FORM_DESC` varchar(64) NOT NULL COMMENT 'The description of the form',
  PRIMARY KEY  (`FORM_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Contains the definition of the various forms in the system'
