CREATE USER 'web'@'localhost' IDENTIFIED BY 'password' WITH MAX_QUERIES_PER_HOUR 0 MAX_CONNECTIONS_PER_HOUR 0 MAX_UPDATES_PER_HOUR 0;

GRANT USAGE ON * . * TO 'web'@'localhost';
GRANT ALL PRIVILEGES ON `world` . * TO 'web'@'localhost' WITH GRANT OPTION;
GRANT ALL PRIVILEGES ON `characters` . * TO 'web'@'localhost' WITH GRANT OPTION;
GRANT ALL PRIVILEGES ON `auth` . * TO 'web'@'localhost' WITH GRANT OPTION;
GRANT ALL PRIVILEGES ON `freedom` . * TO 'web'@'localhost' WITH GRANT OPTION;
GRANT ALL PRIVILEGES ON `dbc_sql` . * TO 'web'@'localhost' WITH GRANT OPTION;
GRANT ALL PRIVILEGES ON `dbo_acc` . * TO 'web'@'localhost' WITH GRANT OPTION;
GRANT ALL PRIVILEGES ON `dbo_char` . * TO 'web'@'localhost' WITH GRANT OPTION;