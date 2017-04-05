/// <reference path="../angular.min.js" />
/// Home

var app = angular.module('Customer', ['ngRoute'])

.factory("WebSqlDbService", function ($q,$rootScope,$http) {
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



    
    service.createDbAndTable = function () {
        var db = openDatabase("IODb", "1.0", 'Mobile Client DB', 2 * 1024 * 1024);
        db.transaction(function (tx) {
            tx.executeSql("Create table if not exists Chart(UserId varchar(50),Id integer (20),Jumlah integer(20))");
        });
        this.ChangeCartCount();

    };

    service.InsertToChart = function (item) {
        var db = openDatabase("IODb", "1.0", 'Mobile Client DB', 2 * 1024 * 1024);
        db.transaction(function (tx) {
            tx.executeSql("Insert into  Chart(UserId,Id,Jumlah) values ('1',?,?)", [item.Id,item.Jumlah]);
        });
       
    };
    service.DeleteItem = function (item) {
        var db = openDatabase("IODb", "1.0", 'Mobile Client DB', 2 * 1024 * 1024);
        db.transaction(function (tx) {
            tx.executeSql("Delete from  Chart where Id=?", [item.Id]);
        });
        this.ChangeCartCount();
    };


    service.ChangeCartCount=function()
    {
        var db = openDatabase("IODb", "1.0", 'Mobile Client DB', 2 * 1024 * 1024);
        db.transaction(function (tx) {
            tx.executeSql("Select * From Chart", [], function (tx, result) {
                $rootScope.CartCount = result.rows.length;
            })
            ;
        })
        $rootScope.$watch($rootScope.CartCount);
    }


   service.GetChart=function () {
       var deferred = $q.defer();
        var data = [];
        var db = openDatabase("IODb", "1.0", 'Mobile Client DB', 2 * 1024 * 1024);
        db.transaction(function (tx) {
            tx.executeSql("Select * From Chart", [], function (tx, result) {
                $rootScope.CartCount = result.rows.length;
                for (var i = 0; i < result.rows.length; i++) {
                    data.push(result.rows.item(i));
                }
                if (data.length > 0) {
                    deferred.resolve(data);
                } else {
                    deferred.reject("Keranjang Pesanan Kosong");
                }

            })
           

            ;
        })
       

        return deferred.promise;
      
    };

   service.ClearCart = function () {
       var db = openDatabase("IODb", "1.0", 'Mobile Client DB', 2 * 1024 * 1024);
       db.transaction(function (tx) {
           tx.executeSql("Delete From Chart", [], function (tx, result) {
           })
           ;
       })
       this.ChangeCartCount();
   };


   service.ReadMessage = function (message) {
       var url = "/api/Inbox/ReadMessage?id=" + message.Id;
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

    return service;

})

.config(function ($routeProvider) {
    $routeProvider.
        when('/Index', {
            templateUrl: 'MenuView.htm',
            controller: 'MenuController'
        }).
     
     when('/Cart', {
         templateUrl: 'CartView.htm',
         controller: 'CartController'
     }).
          when('/Event', {
              templateUrl: 'EventView.htm',
              controller: 'EventController'
          }).
         when('/Payment', {
             templateUrl: 'PaymentView.htm',
             controller: 'PaymentController'
         }).
         when('/Inbox', {
             templateUrl: 'InboxView.htm',
             controller: 'InboxController'
         }).
    otherwise({
        redirectTo: '/Index'
    })
         
    ;

})

 .controller('MenuController', function ($scope, $http, WebSqlDbService) {
     $scope.Judul = "Menu Utama";
     $scope.FilterValue = null;
      $scope.Init = function () {

          WebSqlDbService.createDbAndTable();
         var url = "/api/Customer/GetLayanan";
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
      $scope.AddToChart = function (item) {
          var jumlah = 1;
          if (item.Stok > item.Unit)
          {
              jumlah =parseInt( prompt("Masukkan Jumlah Yang Anda Inginkan :", "0"));
          }
          item.Jumlah = jumlah;
          WebSqlDbService.InsertToChart(item);
          WebSqlDbService.ChangeCartCount();
      };

     $scope.ShowChart=function()
     {
         var value = WebSqlDbService.GetChart();
         var url = "/api/Customer/GetChartView";
         $http({
             method: 'Post',
             url: url,
             data: value
         }).success(
             function (data, status, header, cfg) {
                 alert("Success Insert Data");
             }

         ).error(function (err, status) {
             alert("Cant Not Insert Data");
         });
     }
     $scope.FilterLayanan=function(item)
     {
         $scope.FilterValue = item.Id;
     }

     $scope.ShowCompanyDetail=function(item)
     {
         $scope.ItemDetail = item;
     }

 })


 .controller('CartController', function ($scope, $http, WebSqlDbService,$rootScope) {
     $scope.Judul = "Keranjang Utama";
     $scope.Total = 0;
     $scope.Init = function () {
         $scope.ListLayanan = [];
         $scope.ShowEventButton = false;
         WebSqlDbService.GetChart().then(function (data) {
             var url = "/api/Customer/GetChartView";
             $http({
                 method: 'Post',
                 url: url,
                 data: data
             }).success(
                 function (data, status, header, cfg) {
                     $scope.ListLayanan = data;
                     $scope.CalculateCart();
                     $scope.ShowEventButton = true;
                 }

             ).error(function (err, status) {
                 alert("Data Tidak Ditemukan");
             });
         }, function (msg) {

             alert(msg);
         });
       

     }
    
     $scope.RemoveItem = function (item) {
         var index = $scope.ListLayanan.indexOf(item);
         WebSqlDbService.DeleteItem(item);
         $scope.ListLayanan.splice(index, 1);
         if($scope.ListLayanan.length<=0)
         {
             $scope.ShowEventButton = false;
         }
         WebSqlDbService.ChangeCartCount();
         this.CalculateCart();
         
     }

     $scope.CalculateCart=function()
     {
         $scope.Total = 0;
         if($scope.ListLayanan.length>0)
         {
            
             angular.forEach($scope.ListLayanan, function (value, key) {

                 $scope.Total += (value.Biaya + value.HargaPengiriman);
             })
         }
     }

 })
 .controller('EventController', function ($scope, $http, WebSqlDbService) {
     $scope.Init=function()
     {

     }

     $scope.NewOrder = function (item) {
         WebSqlDbService.GetChart().then(function (data) {

            item.Details = [];
             angular.forEach(data, function (value, key) {
                 var pemesananDetail = {}
                 pemesananDetail.IdPemesanan = 0;
                 pemesananDetail.IdLayanan = value.Id;
                 pemesananDetail.Jumlah = value.Jumlah;
                 pemesananDetail.Penerima = "";
                 item.Details.push(pemesananDetail);

             });

             var url = "/api/Customer/InsertPesanan";
             $http({
                 method: 'Post',
                 url: url,
                 data: item
             }).success(
                 function (data, status, header, cfg) {
                   
                     alert("Sukses,Periksa Inbox Anda dan  Lakukan Pembayaran untuk Prosses Selanjutnya ");
                     WebSqlDbService.ClearCart();
                 }

             ).error(function (err, status) {
                 alert("Cant Not Insert Data");
             });
         }, function (msg) {
             alert(msg);
         });
     }

 })

.controller('PaymentController', function ($scope, $http, WebSqlDbService) {
    $scope.IsLunas = false;
    $scope.IsBatal = false;
    $scope.IsShow = false;
    $scope.IsWaiting = false;
    $scope.ShowForm = false;
    $scope.Pesanan = {};
    $scope.Search=function(code)
    {
        if(code!=null)
        {
            var url = "/api/Customer/GetPesanan?Code=" + code;
            $http({
                method: 'GET',
                url: url,
            }).success(
                function (data, status, header, cfg) {
                    $scope.ListLayanan = data.Items;
                    if (data.pesanan.StatusPesanan==0)
                    {
                        $scope.Pesanan = data.pesanan;
                        $scope.ShowForm = true;
                    } else if (data.pesanan.VerifikasiPembayaran == 1)
                    {
                        $scope.IsLunas = true;
                    } else if (data.pesanan.StatusPesanan == 1 && data.pesanan.VerifikasiPembayaran == 0) {
                        $scope.IsWaiting = true;
                    }else
                    {
                        $scope.IsBatal = true;
                    }

                    $scope.IsShow = true;
                }
            ).error(function (err, status) {
                $scope.ListLayanan = [];
                $scope.IsShow = false;
                alert(err.Message + ", " + status);
            });
        }else
        {
            alert('Masukkan Kode Pesanan');
        }
       
    }
    $scope.Upload=function()
    {
        var f = document.getElementById("file");
        var res = f.files[0];

        var form = new FormData();

        form.append("file", res);
        form.append("IdPemesanan", $scope.Pesanan.Id);
        form.append("Bank", $scope.model.Bank);
        form.append("TipeFile", res.type);
        form.append("Pesan", $scope.model.Pesan);
        form.append("NamaPengirim", $scope.model.NamaPengirim);
        var settings = {
            "async": true,
            "crossDomain": true,
            "url": "/api/Customer/PaymentConfirm",
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
            console.log(response);
            if (data == "success") {
                alert("Berhasil menambah data");
            } else {
                alert("Gagal Menambahkan data");
            }
        });
    }
})


.controller('InboxController', function ($scope, $http, WebSqlDbService) {

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


    $scope.ShowMessage=function(item)
    {
        $scope.SelectedMessage = item;
        WebSqlDbService.ReadMessage(item);
    }



})

;

