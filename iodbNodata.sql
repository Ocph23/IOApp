# Host: localhost  (Version: 5.6.14)
# Date: 2017-03-19 14:21:23
# Generator: MySQL-Front 5.3  (Build 4.4)

/*!40101 SET NAMES utf8 */;

#
# Source for table "aspnetroles"
#

DROP TABLE IF EXISTS `aspnetroles`;
CREATE TABLE `aspnetroles` (
  `Id` varchar(128) NOT NULL,
  `Name` varchar(256) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Source for table "aspnetusers"
#

DROP TABLE IF EXISTS `aspnetusers`;
CREATE TABLE `aspnetusers` (
  `Id` varchar(128) NOT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEndDateUtc` datetime DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `UserName` varchar(256) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Source for table "aspnetuserroles"
#

DROP TABLE IF EXISTS `aspnetuserroles`;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(128) NOT NULL,
  `RoleId` varchar(128) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IdentityRole_Users` (`RoleId`),
  CONSTRAINT `ApplicationUser_Roles` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `IdentityRole_Users` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Source for table "aspnetuserlogins"
#

DROP TABLE IF EXISTS `aspnetuserlogins`;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `UserId` varchar(128) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`),
  KEY `ApplicationUser_Logins` (`UserId`),
  CONSTRAINT `ApplicationUser_Logins` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Source for table "aspnetuserclaims"
#

DROP TABLE IF EXISTS `aspnetuserclaims`;
CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(128) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `ApplicationUser_Claims` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Source for table "kategori"
#

DROP TABLE IF EXISTS `kategori`;
CREATE TABLE `kategori` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nama` varchar(255) DEFAULT NULL,
  `Keterangan` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

#
# Source for table "layanan"
#

DROP TABLE IF EXISTS `layanan`;
CREATE TABLE `layanan` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nama` varchar(255) NOT NULL DEFAULT '',
  `IdKategori` int(11) NOT NULL DEFAULT '0',
  `IdPerusahaan` int(11) NOT NULL DEFAULT '0',
  `Stok` int(11) NOT NULL DEFAULT '0',
  `Unit` double NOT NULL DEFAULT '0',
  `Harga` double NOT NULL DEFAULT '0',
  `HargaPengiriman` double NOT NULL DEFAULT '0',
  `Keterangan` text,
  `Aktif` enum('true','false') NOT NULL DEFAULT 'true',
  `Photo` longblob,
  PRIMARY KEY (`Id`),
  KEY `IdPerusahaan` (`IdPerusahaan`),
  KEY `IdKategori` (`IdKategori`),
  CONSTRAINT `layanan_ibfk_1` FOREIGN KEY (`IdKategori`) REFERENCES `kategori` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

#
# Source for table "foto"
#

DROP TABLE IF EXISTS `foto`;
CREATE TABLE `foto` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdLayanan` int(11) NOT NULL DEFAULT '0',
  `NamaFile` varchar(100) NOT NULL DEFAULT '',
  `TipeFile` varchar(100) NOT NULL DEFAULT '',
  `data` longblob NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IdLayanan` (`IdLayanan`),
  CONSTRAINT `foto_ibfk_1` FOREIGN KEY (`IdLayanan`) REFERENCES `layanan` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

#
# Source for table "pelanggan"
#

DROP TABLE IF EXISTS `pelanggan`;
CREATE TABLE `pelanggan` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nama` varchar(255) NOT NULL DEFAULT '',
  `UserId` varchar(255) NOT NULL DEFAULT '',
  `Email` varchar(255) NOT NULL DEFAULT '',
  `Telepon` varchar(255) DEFAULT NULL,
  `Alamat` varchar(255) DEFAULT NULL,
  `Tanggal` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `pelanggan_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

#
# Source for table "pemesanan"
#

DROP TABLE IF EXISTS `pemesanan`;
CREATE TABLE `pemesanan` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `KodePemesanan` varchar(20) DEFAULT NULL,
  `IdPelanggan` int(11) DEFAULT NULL,
  `TanggalAcara` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `Alamat` text NOT NULL,
  `Catatan` text,
  `StatusPesanan` enum('Baru','Menunggu','Pelaksanaan','Selesai','Batal') NOT NULL DEFAULT 'Baru',
  `Tanggal` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `VerifikasiPembayaran` enum('Tunda','Lunas','Batal') NOT NULL DEFAULT 'Tunda',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;

#
# Source for table "pembayaran"
#

DROP TABLE IF EXISTS `pembayaran`;
CREATE TABLE `pembayaran` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdPemesanan` int(11) NOT NULL DEFAULT '0',
  `bank` varchar(255) NOT NULL DEFAULT '',
  `NamaPengirim` varchar(255) NOT NULL DEFAULT '',
  `Pesan` varchar(255) NOT NULL DEFAULT '',
  `NamaFile` varchar(100) NOT NULL DEFAULT '',
  `TipeFile` varchar(100) NOT NULL DEFAULT '',
  `data` longblob,
  PRIMARY KEY (`Id`),
  KEY `IdPemesanan` (`IdPemesanan`),
  CONSTRAINT `pembayaran_ibfk_1` FOREIGN KEY (`IdPemesanan`) REFERENCES `pemesanan` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

#
# Source for table "pelaksanaan"
#

DROP TABLE IF EXISTS `pelaksanaan`;
CREATE TABLE `pelaksanaan` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdPesanan` int(11) NOT NULL DEFAULT '0',
  `IdLayanan` int(11) NOT NULL DEFAULT '0',
  `Pengantaran` datetime DEFAULT NULL,
  `DiantarOleh` varchar(255) DEFAULT NULL,
  `DiterimaOleh` varchar(255) DEFAULT NULL,
  `Dikembalikan` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IdPesanan` (`IdPesanan`),
  KEY `IdLayanan` (`IdLayanan`),
  CONSTRAINT `pelaksanaan_ibfk_1` FOREIGN KEY (`IdPesanan`) REFERENCES `pemesanan` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

#
# Source for table "pemesanandetail"
#

DROP TABLE IF EXISTS `pemesanandetail`;
CREATE TABLE `pemesanandetail` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdPemesanan` int(11) DEFAULT NULL,
  `IdLayanan` int(11) DEFAULT NULL,
  `Jumlah` int(11) NOT NULL DEFAULT '0',
  `Diantar` enum('true','false') NOT NULL DEFAULT 'false',
  `Penerima` varchar(255) NOT NULL DEFAULT '',
  `Kembali` enum('true','false') NOT NULL DEFAULT 'false',
  PRIMARY KEY (`Id`),
  KEY `TransactionId` (`IdPemesanan`),
  KEY `IdLayanan` (`IdLayanan`),
  CONSTRAINT `pemesanandetail_ibfk_1` FOREIGN KEY (`IdPemesanan`) REFERENCES `pemesanan` (`Id`),
  CONSTRAINT `pemesanandetail_ibfk_2` FOREIGN KEY (`IdLayanan`) REFERENCES `layanan` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=latin1;

#
# Source for table "perusahaan"
#

DROP TABLE IF EXISTS `perusahaan`;
CREATE TABLE `perusahaan` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) NOT NULL DEFAULT '',
  `Nama` varchar(255) DEFAULT NULL,
  `Pemilik` varchar(255) DEFAULT NULL,
  `Telepon` varchar(255) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Alamat` varchar(255) DEFAULT NULL,
  `Tanggal` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Terverifikasi` enum('false','true') NOT NULL DEFAULT 'false',
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `perusahaan_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

#
# Source for table "pesan"
#

DROP TABLE IF EXISTS `pesan`;
CREATE TABLE `pesan` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` varchar(128) NOT NULL DEFAULT '',
  `Pengirim` varchar(255) NOT NULL DEFAULT '',
  `Judul` varchar(255) NOT NULL DEFAULT '',
  `Tanggal` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `Pesan` longtext NOT NULL,
  `Terbaca` enum('true','false') NOT NULL DEFAULT 'false',
  PRIMARY KEY (`Id`),
  KEY `IdUser` (`IdUser`),
  CONSTRAINT `pesan_ibfk_1` FOREIGN KEY (`IdUser`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

#
# Source for view "reportfee"
#

DROP VIEW IF EXISTS `reportfee`;
CREATE VIEW `reportfee` AS 
  select `iodb`.`pemesanan`.`KodePemesanan` AS `KodePemesanan`,`iodb`.`pemesanan`.`StatusPesanan` AS `StatusPesanan`,`iodb`.`pemesanan`.`Tanggal` AS `Tanggal`,`iodb`.`pemesanan`.`VerifikasiPembayaran` AS `VerifikasiPembayaran`,`iodb`.`pemesanandetail`.`Jumlah` AS `Jumlah`,`iodb`.`layanan`.`Unit` AS `Unit`,`iodb`.`perusahaan`.`Nama` AS `Perusahaan`,`iodb`.`pelanggan`.`Nama` AS `Pelanggan`,((`iodb`.`pemesanandetail`.`Jumlah` * `iodb`.`layanan`.`Harga`) + `iodb`.`layanan`.`HargaPengiriman`) AS `biaya`,((((`iodb`.`pemesanandetail`.`Jumlah` * `iodb`.`layanan`.`Harga`) + `iodb`.`layanan`.`HargaPengiriman`) * 20) / 100) AS `fee`,`iodb`.`layanan`.`Nama` AS `NamaLayanan` from ((((`iodb`.`pemesanan` left join `iodb`.`pemesanandetail` on((`iodb`.`pemesanan`.`Id` = `iodb`.`pemesanandetail`.`IdPemesanan`))) left join `iodb`.`layanan` on((`iodb`.`pemesanandetail`.`IdLayanan` = `iodb`.`layanan`.`Id`))) left join `iodb`.`perusahaan` on((`iodb`.`layanan`.`IdPerusahaan` = `iodb`.`perusahaan`.`Id`))) left join `iodb`.`pelanggan` on((`iodb`.`pemesanan`.`IdPelanggan` = `iodb`.`pelanggan`.`Id`)));

#
# Source for trigger "InsertUserInRole"
#

DROP TRIGGER IF EXISTS `InsertUserInRole`;
CREATE DEFINER='root'@'localhost' TRIGGER `iodb`.`InsertUserInRole` AFTER INSERT ON `iodb`.`aspnetusers`
  FOR EACH ROW Insert into aspnetuserroles (UserId, RoleId) values (new.Id, "4");

#
# Source for trigger "updaterolesfromcompany"
#

DROP TRIGGER IF EXISTS `updaterolesfromcompany`;
CREATE DEFINER='root'@'localhost' TRIGGER `iodb`.`updaterolesfromcompany` AFTER INSERT ON `iodb`.`perusahaan`
  FOR EACH ROW update aspnetuserroles set RoleId='2' where UserId=new.UserId;

#
# Source for trigger "updaterolesfromcustomer"
#

DROP TRIGGER IF EXISTS `updaterolesfromcustomer`;
CREATE DEFINER='root'@'localhost' TRIGGER `iodb`.`updaterolesfromcustomer` AFTER INSERT ON `iodb`.`pelanggan`
  FOR EACH ROW update aspnetuserroles set RoleId='3' where UserId=new.UserId;
