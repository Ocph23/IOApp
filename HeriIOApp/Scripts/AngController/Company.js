/// <reference path="../angular.min.js" />
/// Home

var app = angular.module('Company', ['ngRoute'])
.factory("CompanyService", function ($rootScope,$http) {
    var service = {};
    var url = "/api/Inbox/GetInboxCount";
    $http({
        method: 'GET',
        url: url,
    }).success(
        function (data, status, header, cfg) {
            $rootScope.InboxCount = data;
        }
    ).error(function (err, status) {
        $rootScope.InboxCount = 0;
    });


    service.ReadMessage = function (message) {
        var url = "/api/Inbox/ReadMessage?id="+message.Id;
        $http({
            method: 'Get',
            url: url,
        }).success(
            function (data, status, header, cfg) {
                message.Terbaca = true;
            }
        ).error(function (err, status) {
            $rootScope.InboxCount = 0;
        });

    }

    service.ConvertStatusPesanan = function (value) {

        if (value == 0)
            return "Baru";
        else if (value == 1)
            return "Menunggu";
        else if (value == 2)
            return "Pelaksanaan";
        else if (value == 2)
            return "Selesai";
        else
            return "Batal";

    }

    service.ConvertVerifikasiPembayaran = function (value) {

        if (value == 0)
            return "Tunda";
        else if (value == 1)
            return "Lunas";
        else (value == 2)
        return "Batal";
    }


    return service;

})
.config(function ($routeProvider) {
    $routeProvider.
        when('/Index', {
            templateUrl: 'MenuView.htm',
            controller: 'MenuController'
        }).
         when('/Layanan', {
             templateUrl: 'LayananView.htm',
             controller: 'LayananController'
         }).

          when('/Pesanan', {
              templateUrl: 'PesananView.htm',
              controller: 'PesananController'
          }).
      when('/Inbox', {
          templateUrl: 'InboxView.htm',
          controller: 'InboxController'
      });

})

 .controller('MenuController', function ($scope, $http,CompanyService) {
     $scope.Judul = "Menu Utama";
 })

    .controller('LayananController', function ($scope, $http, CompanyService) {
        $scope.Judul = "Daftar Layanan";
        $scope.ListLayanan = [];
        $scope.Init = function () {
            var url = "/api/Company/GetLayanan";
            $http({
                method: 'GET',
                url: url,
            }).success(
                function (data, status, header, cfg) {
                    $scope.ListLayanan = data;
                }
            ).error(function (err, status) {
                alert(err.Message + ", " + status);
            });

            $scope.Categories = [];
            var url = "/api/Categories/Get";
            $http({
                method: 'GET',
                url: url,
            }).success(
                function (data, status, header, cfg) {
                    $scope.Categories = data;
                }
            ).error(function (err, status) {
                alert(err.Message + ", " + status);
            });
        }
        $scope.IsNew = true;

      
        $scope.SaveLayanan=function(value)
        {
            var f = document.getElementById("file");
            var res = f.files[0];
            var form = new FormData();
            form.append("file", res);
            form.append("Nama", value.Nama);
            form.append("IdKategori", value.IdKategori);
            form.append("Stok", value.Stok);
            form.append("Unit", value.Unit);
            form.append("Harga", value.Harga);
            form.append("HargaPengiriman", value.HargaPengiriman);
            form.append("Keterangan", value.Keterangan);

            var settings = {
                "async": true,
                "crossDomain": true,
                "url": "/api/Company/PostLayanan",
                "method": "POST",
                "headers": {
                    "cache-control": "no-cache",
                },
                "processData": false,
                "contentType": false,
                "mimeType": "multipart/form-data",
                "data": form
            }

            $.ajax(settings).done(function (response, data) {
                value.Id = response;
                $scope.ListLayanan.push(value)
                alert("Berhasil menambah data");
            }).error(function (response, data) {
                alert(response.responseText);


            });


          
        }


        $scope.RemoveLayanan = function (item) {
            var url = "/api/Company/RemoveLayanan?Id="+item.Id+"&actived="+item.Aktif;
            $http({
                method: 'Post',
                url: url,
            }).success(
                function (data, status, header, cfg) {
                    $scope.Categories = data;
                }
            ).error(function (err, status) {
                alert(err.Message + ", " + status);
            });

        }


        $scope.ChangeLayananActived = function (item) {
            var url = "/api/Company/ChangeLayananActived";
            $http({
                method: 'Post',
                url: url,
                data:item
            }).success(
                function (data, status, header, cfg) {
                    alert("Data Telah Diubah");
                }
            ).error(function (err, status) {
                alert(err.Message + ", " + status);
                item.Aktif = !item.Aktif;
            });
        }

    })


 .controller('PesananController', function ($scope, $http, CompanyService) {
     $scope.Judul = "Daftar Pesanan";
     $scope.Init = function () {
         var url = "/api/Company/GetPesanan";
         $http({
             method: 'GET',
             url: url,
         }).success(
             function (data, status, header, cfg) {
                 $scope.Pesanan = data;
                 angular.forEach($scope.Pesanan, function (value, key) {
                     value.StatusPesananText = CompanyService.ConvertVerifikasiPembayaran(value.VerifikasiPembayaran);
                 })
             }


         ).error(function (err, status) {
             alert(err.Message + ", " + status);
         });
     }


     $scope.ChangeProgress=function(item)
     {
         var url = "/api/Company/ChangeProgress";
         $http({
             method: 'post',
             url: url,
             data:item
         }).error(function (err, status) {
         });
     }
    


 })

 .controller('InboxController', function ($scope, $http, CompanyService) {
     $scope.Judul = "Inbox";
     $scope.MessageISShow = false;
     $scope.Init = function () {
         var url = "/api/Inbox/GetInbox";
         $http({
             method: 'GET',
             url: url,
         }).success(
             function (data, status, header, cfg) {
                 $scope.Inbox = data

             });
     };


     $scope.ShowMessage = function (item) {
         $scope.SelectedMessage = item;
         CompanyService.ReadMessage(item);
     }
 })


;

