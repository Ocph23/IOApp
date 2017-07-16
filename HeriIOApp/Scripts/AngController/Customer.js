/// <reference path="../angular.min.js" />
/// Home

var app = angular.module('Customer', ['ngRoute','angular.filter','angularSpinner'])
    .directive('dir', function ($compile, $parse) {
        return {
            restrict: 'E',
            link: function (scope, element, attr) {
                scope.$watch(attr.content, function () {
                    element.html($parse(attr.content)(scope));
                    $compile(element.contents())(scope);
                }, true);
            }
        }
    })  
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
            tx.executeSql("Create table if not exists Chart(UserId varchar(50),Id integer (20) UNIQUE,Jumlah integer(20))");
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
            });
        });
        return deferred.promise;
      
    };

   service.ClearCart = function () {
       var db = openDatabase("IODb", "1.0", 'Mobile Client DB', 2 * 1024 * 1024);
       db.transaction(function (tx) {
           tx.executeSql("Delete From Chart", [], function (tx, result) {
           });
       });
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

   };
    return service;
})

.config(function ($routeProvider) {
    $routeProvider.
        when('/Index', {
            templateUrl: 'MenuView.htm',
            controller: 'MenuController'
        }).
        
       
          when('/Perusahaan/:Id', {
              templateUrl: 'Perusahaan.htm',
              controller: 'PerusahaanController'
          }).

          when('/LayananDetail/:Id', {
              templateUrl: 'LayananDetail.htm',
              controller: 'LayananDetailController'
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
          when('/MyEvent', {
              templateUrl: 'MyEventView.htm',
              controller: 'AddEventController'
          }).
         when('/MyEventDetail', {
             templateUrl: 'MyEventDetailView.htm',
             controller: 'AddEventController'
         }).
         when('/AddEvent', {
             templateUrl: 'AddEventView.htm',
             controller: 'AddEventController'
         }).
    otherwise({
        redirectTo: '/Index'
    })
         
    ;

})

 .controller('MenuController', function ($scope, $http, WebSqlDbService,$route) {
     $scope.Judul = "Menu Utama";
     $scope.FilterValue =0;
     $scope.Init = function () {
         var url = "";
         WebSqlDbService.createDbAndTable();
         url = "/api/Customer/GetLayanan";
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


         url = "/api/Categories/Get";
         $http({
             method: 'GET',
             url: url
         }).success(
             function (data, status, header, cfg) {
                 $scope.Categories = data;
             }
             ).error(function (err, status) {
                 alert(err.Message + ", " + status);
             });
     };

      $scope.AddToChart = function (item) {

          var jumlah = 1;
          if (item.Stok > 1)
          {
              jumlah = parseInt(prompt("Masukkan Jumlah Yang Anda Inginkan :", "0"));
              if (!isNaN( jumlah) &&jumlah > item.Stok)
              {
                  alert("Permintaan Anda Melebihi Stock, Tersedia " + item.Stok);
              } else if (!isNaN(jumlah))
              {
                  item.Jumlah = jumlah;
                  WebSqlDbService.InsertToChart(item);
                  WebSqlDbService.ChangeCartCount();
                  $route.reload();
                  alert("Permintaan Anda Telah Ditambhakan");
              }

          }else
          {
              item.Jumlah = jumlah;
              WebSqlDbService.InsertToChart(item);
              WebSqlDbService.ChangeCartCount();
              $route.reload();
          }
        
         
      };

      $scope.ShowChart = function () {
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
      };
      $scope.FilterLayanan = function (item) {
          $scope.FilterValue = item.Id;
      };

      $scope.ShowCompanyDetail = function (item) {
          $scope.ItemDetail = item;
      };

 })

 .controller('PerusahaanController', function ($scope, $http, WebSqlDbService, $route, $routeParams,$sce) {
     $scope.IsShowDescription = false;
     $scope.Init = function () {
         $scope.IsShowDescription = false;
         var url = "/api/Customer/GetCompanyProfile?Id=" + $routeParams.Id;
         $http({
             method: 'GET',
             url: url,
         }).success(
             function (data, status, header, cfg) {
                 $scope.perusahaan = data.data;
                 $scope.Profile = data.pro;
                 $scope.Profile.Selogan = $sce.trustAsHtml(data.pro.Selogan);
                 $scope.Profile.Description = $sce.trustAsHtml(data.pro.Description);

             }
         ).error(function (err, status) {
             alert(err.Message + ", " + status);
         });
     };

     $scope.ShowDescription = function () {
         $scope.IsShowDescription = true;
     };
 })

 .controller('LayananDetailController', function ($scope, $http, WebSqlDbService, $route, $routeParams,$sce) {
     var url = "/api/Customer/GetLayananDetail?Id=" + $routeParams.Id;
     $http({
         method: 'GET',
         url: url,
     }).success(
         function (data, status, header, cfg) {
             angular.forEach(data.Layanans, function (value, key) {
                 value.HtmlText = $sce.trustAsHtml(value.Keterangan);
             })
             $scope.perusahaan = data;
         }
     ).error(function (err, status) {
         alert(err.Message + ", " + status);
     });

     $scope.GetHtmlText=function(value)
     {
         return $sce.trustAsHtml(value);
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

                 $scope.Total += (value.Biaya);
             })
         }
     }

    })

 .controller('EventController', function ($scope, $http, WebSqlDbService, usSpinnerService) {
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
                 alert(err.Message);
             });
         }, function (msg) {
             alert(msg);
         });
      
     }

    })

.controller('AddEventController', function ($scope, $http, WebSqlDbService, $location, $rootScope, usSpinnerService,$sce) {
    $scope.startSpin = function () {
        usSpinnerService.spin('spinner-1');
    }
    $scope.stopSpin = function () {
        usSpinnerService.stop('spinner-1');
    }
    $scope.SyaratIsShow = false;
    $scope.Init = function () {
        $scope.SyartIsShow = false;
        var url = "/api/Events/Get";
        $http({
            method: 'Get',
            url: url
        }).success(
            function (data, status, header, cfg) {
                $scope.EventTipes = data;
            }

        ).error(function (err, status) {
            alert(err.Message);
        });


        url = "/api/Customer/GetMyEvents";
        $http({
            method: 'Get',
            url: url
        }).success(
            function (data, status, header, cfg) {
                $scope.Events = data;
            }

        ).error(function (err, status) {
            alert(err.Message);
        });
    };

    $scope.SyaratIsShowAction = function () {
        $scope.SyaratIsShow = true;
    };

    $scope.NewOrder = function (item) {
        $scope.startSpin();
        try {
            item.JenisEventId = $scope.JenisEventSelected.Id;
            if (item.TanggalAcara <= new Date() ||
                item.TanggalAcara >= item.TanggalSelesai ||
                item.JenisEventId <= 1) {
                $scope.stopSpin();
                alert("Periksa Kembali Data Anda");
            } else {
                var url = "/api/Customer/InsertEvent";
                $http({
                    method: 'Post',
                    url: url,
                    data: item
                }).success(
                    function (data, status, header, cfg) {
                        $scope.stopSpin();
                        alert("Sukses,Periksa Inbox Anda dan  Lakukan Pembayaran untuk Prosses Selanjutnya ");
                    }

                    ).error(function (err, status) {
                        $scope.stopSpin();
                        alert(err.Message + " Check Internet Connection");
                    });
            }
        } catch (e) {
            alert("Periksa Kembali Data Anda");
            $scope.stopSpin();
        }
       
    };

    $scope.SimpanPenawaran=function()
    {
        var url = "/api/Customer/SimpanPenawaran";
        $http({
            method: 'POST',
            url: url,
            data:$scope.Penawarans
        }).success(
            function (data, status, header, cfg) {
               

            }
        ).error(function (err, status) {
           
            alert(err.Message + ", " + status);
        });
    }

    $scope.InitView=function()
    {
        if ($rootScope.Pesanan !== undefined) {
            var url = "/api/Customer/GetPenawarans?id=" + $rootScope.Pesanan.Id;
            $http({
                method: 'GET',
                url: url,
            }).success(
                function (data, status, header, cfg) {
                    $scope.Penawarans = data;

                }
            ).error(function (err, status) {
                $scope.ListLayanan = [];
                $scope.IsShow = false;
                alert(err.Message + ", " + status);
            });
        }else
        {
            $location.path("MyEvent");
        }
      
    }

    $scope.SelectAllAction=function(item)
    {
        if($scope.Penawarans!=undefined)
        {
            angular.forEach($scope.Penawarans, function (value, key) {
                value.Dipilih = item;
            });

        }
    }

    $scope.LihatPenawaran = function(item)
    {
        $rootScope.Pesanan = item;
        $location.path("MyEventDetail");
    }

    $scope.ShowPenawaran=function(item)
    {
        $scope.Penawaran = {};
        $scope.Penawaran = item;
        $scope.Penawaran.DetailPenawaranHtml = $sce.trustAsHtml(item.DetailPenawaran)
    }

    $scope.CancelAction = function(item)
    {
        var url = "/api/Customer/CancelAction";
        $http({
            method: 'POST',
            url: url,
            data:item
        }).success(
            function (data, status, header, cfg) {
                item.StatusPesanan = "Batal";
                item.VerifikasiPembayaran = "Batal";
            }
        ).error(function (err, status) {
            alert(err.Message);
        });
    }
    })

.controller('PaymentController', function ($scope, $http, WebSqlDbService) {
    $scope.IsLunas = false;
    $scope.IsBatal = false;
    $scope.IsShow = false;
    $scope.IsWaiting = false;
    $scope.ShowForm = false;
    $scope.IsEvent = false;
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
                    $scope.Pesanan = data;
                    $scope.IsShow = true;
                    if (!data.IsEvent) {
                        if (data.Layanans.length <= 0) {
                            alert('Anda Belum Memilih Layanan');
                        } else {
                            $scope.ListLayanan = data.Layanans;
                            if (data.VerifikasiPembayaran === "None") {
                                $scope.Pesanan = data;
                                $scope.ShowForm = true;
                            } else if (data.VerifikasiPembayaran === "Lunas") {
                                $scope.IsLunas = true;
                            } else if (data.VerifikasiPembayaran === "Panjar") {
                                $scope.IsPanjar = true;
                                $scope.ShowForm = true;
                            } else if (data.VerifikasiPembayaran === "MenungguVerifikasi") {
                                $scope.IsWaiting = true;
                            } else {
                                $scope.IsBatal = true;
                            }
                        };

                        $scope.Total = 0;
                        angular.forEach($scope.ListLayanan, function (value, key) {
                            $scope.Total += value.Biaya;
                        });
                       
                    } else if(data.IsEvent)
                    {
                        if (data.Penawarans.length <= 0) {
                            alert('Anda Belum Memilih Penawaran');
                        } else {
                            $scope.IsEvent = true;
                            $scope.ListLayanan = data.Penawarans;
                          //  $scope.ListLayanan = data.Layanans;
                            if (data.VerifikasiPembayaran === "None") {
                                $scope.Pesanan = data;
                                $scope.ShowForm = true;
                            } else if (data.VerifikasiPembayaran === "Lunas") {
                                $scope.IsLunas = true;
                            } else if (data.VerifikasiPembayaran === "Panjar") {
                                $scope.IsPanjar = true;
                                $scope.ShowForm = true;
                            } else if (data.VerifikasiPembayaran === "MenungguVerifikasi") {
                                $scope.IsWaiting = true;
                            } else {
                                $scope.IsBatal = true;
                            };

                            $scope.Total = 0;
                            angular.forEach($scope.ListLayanan, function (value, key) {
                                $scope.Total += value.Biaya;
                            });
                        }
                    }
                  
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
        form.append("JumlahBayar", $scope.model.JumlahBayar);

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
        };

      

        var t=$scope.Total+$scope.Pesanan.KodeValidasi;
        var minPanjar = (t*50)/100;
        if($scope.model.JumlahBayar<t && $scope.Pesanan.VerifikasiPembayaran=="None")
        {
            form.append("JenisPembayaran", "Panjar");
            if($scope.model.JumlahBayar<minPanjar)
            {
                alert("Nilai Panjar Minimum 50 % dari Total Pembayaran. (Rp.  " + minPanjar + ")");
            }else
            {
                $.ajax(settings).done(function (response, data) {
                    console.log(response);
                    if (data === "success") {
                        alert("Berhasil menambah data");
                        $scope.ShowForm = false;
                    } else {
                        alert("Gagal Menambahkan data");
                    }
                });
            }
        } else if ($scope.model.JumlahBayar == t && $scope.Pesanan.VerifikasiPembayaran == "None")
        {
            form.append("JenisPembayaran", "Pelunasan");
            $.ajax(settings).done(function (response, data) {
                console.log(response);
                if (data === "success") {
                    alert("Berhasil menambah data");
                    $scope.ShowForm = false;
                } else {
                    alert("Gagal Menambahkan data");
                }
            });
        }else if($scope.Pesanan.VerifikasiPembayaran == "Panjar")
        {
            t = $scope.Total + $scope.Pesanan.KodeValidasi;
            var sisa =t- $scope.Pesanan.Panjar.JumlahBayar;
            if(sisa != $scope.model.JumlahBayar)
            {
                alert("Sisa Tagihan Anda Seharusnya. (Rp.  " + sisa+ ")");
            } else
            {
                form.append("JenisPembayaran", "Pelunasan");
                $.ajax(settings).done(function (response, data) {
                    console.log(response);
                    if (data === "success") {
                        alert("Berhasil menambah data");
                        $scope.ShowForm = false;
                    } else {
                        alert("Gagal Menambahkan data");
                    }
                });
            }
          
        }

      
      
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

