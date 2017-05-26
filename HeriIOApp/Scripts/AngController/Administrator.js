/// <reference path="../angular.min.js" />
/// Home

var app = angular.module('Administrator', ['ngRoute'])

.config(function ($routeProvider) {
    $routeProvider.
        when('/', {
            templateUrl: 'CategoriesView.htm',
            controller: 'CategoriesController'
        }).
         when('/Pengusaha', {
             templateUrl: 'PengusahaView.htm',
             controller: 'PengusahaController'
         }).

          when('/Pelanggan', {
              templateUrl: 'PelangganView.htm',
              controller: 'PelangganController'
          }).
         when('/Pesanan', {
             templateUrl: 'PesananView.htm',
             controller: 'PesananController'
         }).
           when('/Pembayaran', {
               templateUrl: 'PembayaranView.htm',
               controller: 'PembayaranController'
           }).
        when('/LaporanPenjualan', {
            templateUrl: 'LaporanPenjualanView.htm',
            controller: 'LaporanPenjualanController'
        }).
      when('/Categories', {
          templateUrl: 'CategoriesView.htm',
          controller: 'CategoriesController'
      }).
        when('/Events', {
          templateUrl: 'EventsView.htm',
          controller: 'EventsController'
      });
    
})

 .controller('MenuController', function ($scope, $http) {
     $scope.Judul = "SELAMAT DATANG";
 })

    .controller('CategoriesController', function ($scope, $http) {
        $scope.Judul = "Daftar Kategori";
        $scope.CategoriIsNew = true;
        $scope.category = {};

        $scope.Init=function()
        {
            $scope.CategoriIsNew = true;
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

        $scope.ChangeNewToedit = function(item)
        {
            $scope.category.Id = item.Id;
            $scope.category.Nama = item.Nama;
            $scope.category.Keterangan = item.Keterangan;
            $scope.CategoriIsNew = false;
        }

        $scope.SaveCategory = function (value) {
            if ($scope.CategoriIsNew) {
                value.Id = 0;
                value.Objects = [];
                var url = "/api/Categories/post";
                $http({
                    method: 'Post',
                    url: url,
                    data: value
                }).success(
                    function (data, status, header, cfg) {
                        value.Id = data;
                        $scope.Categories.push(value)
                        alert("Success Insert Data");
                    }

                ).error(function (err, status) {
                    alert("Cant Not Insert Data");
                });
            } else {
                var url = "/api/Categories/put";
                $http({
                    method: 'put',
                    url: url,
                    data: value
                }).success(
                    function (data, status, header, cfg) {
                        value.Id = data;
                        $scope.Categories.push(value)
                        alert("Success Update");
                    }

                ).error(function (err, status) {
                    alert("Cant Not Update Data");
                });
            }

        }

        $scope.DeleteItem = function(value)
        {
            var url = "/api/Categories/DeleteCategory";
            $http({
                method: 'post',
                url: url,
                data: value
            }).success(
                function (data, status, header, cfg) {
                    var index = $scope.Categories.indexOf(value);
                    $scope.Categories.splice(index, 1);
                    alert("Success Delete Data");
                }

            ).error(function (err, status) {
                alert("Cant Not Delete Data");
            });
        }

    })

      .controller('EventsController', function ($scope, $http) {
          $scope.Judul = "Daftar Event";
          $scope.EventIsNew = true;
          $scope.event = {};

          $scope.Init = function () {
              $scope.EventIsNew= true;
              $scope.Events = [];
              var url = "/api/Events/Get";
              $http({
                  method: 'GET',
                  url: url,
              }).success(
                  function (data, status, header, cfg) {
                      $scope.Events = data;
                  }


              ).error(function (err, status) {
                  alert(err.Message + ", " + status);
              });
          }

          $scope.ChangeNewToedit = function (item) {
              $scope.event.Id = item.Id;
              $scope.event.Nama = item.Nama;
              $scope.event.Keterangan = item.Keterangan;
              $scope.EventIsNew= false;
          }

          $scope.SaveEvent = function (value) {
              if ($scope.EventIsNew) {
                  value.Id = 0;
                  value.Objects = [];
                  var url = "/api/Events/post";
                  $http({
                      method: 'Post',
                      url: url,
                      data: value
                  }).success(
                      function (data, status, header, cfg) {
                          value.Id = data;
                          $scope.Events.push(value)
                          alert("Success Insert Data");
                      }

                  ).error(function (err, status) {
                      alert("Cant Not Insert Data");
                  });
              } else {
                  var url = "/api/Events/put";
                  $http({
                      method: 'put',
                      url: url,
                      data: value
                  }).success(
                      function (data, status, header, cfg) {
                          value.Id = data;
                          alert("Success Update");
                      }

                  ).error(function (err, status) {
                      alert("Cant Not Update Data");
                  });
              }

          }

          $scope.DeleteItem = function (value) {
              var url = "/api/Categories/DeleteCategory";
              $http({
                  method: 'post',
                  url: url,
                  data: value
              }).success(
                  function (data, status, header, cfg) {
                      var index = $scope.Events.indexOf(value);
                      $scope.Events.splice(index, 1);
                      alert("Success Delete Data");
                  }

              ).error(function (err, status) {
                  alert("Cant Not Delete Data");
              });
          }

      })

 .controller('PengusahaController', function ($scope, $http) {
     $scope.Judul = "Daftar Pengusaha";
     $scope.Company= {};
     $scope.Companies= [];
     $scope.Init = function () {
         
         var url = "/api/Administrator/GetCompanies";
         $http({
             method: 'GET',
             url: url,
         }).success(
             function (data, status, header, cfg) {
                 $scope.Companies= data;
             }
         ).error(function (err, status) {
             alert(err.Message + ", " + status);
         });
     }



     $scope.ShowCompanyDetail=function(item)
     {
         $scope.ItemDetail = item;
     }


     $scope.ValidateCompany=function(item)
     {

         var data = item;
         var url = "/api/Administrator/ValidateCompany";
         $http({
             method: 'Post',
             url: url,
             data: data
         }).success(
             function (data, status, header, cfg) {
                 alert(data);
                
             }

         ).error(function (err, status) {
             alert(err.Message);
             item.Terverifikasi = !item.Terverifikasi;
         });
     }

 })


 .controller('PelangganController', function ($scope, $http) {
     $scope.Judul = "Daftar Pelanggan";
     $scope.Customers= [];
     $scope.Init = function () {

         var url = "/api/Administrator/GetCustomers";
         $http({
             method: 'GET',
             url: url,
         }).success(
             function (data, status, header, cfg) {
                 $scope.Customers= data;
             }
         ).error(function (err, status) {
             alert(err.Message + ", " + status);
         });
     }



     $scope.ShowCustomerDetail = function (item) {
         $scope.ItemDetail = item;
     }

 })


 .controller('PesananController', function ($scope, $http) {
     $scope.Judul = "Pesanan";
     $scope.Verifikasi = [{ Id: 0, Nama: "Tunda" }, { Id: 1, Nama: "Lunas" }, { Id: 2, Nama: "Batal" }]
     $scope.Init = function () {
         $scope.Categories = [];
         var url = "/api/Administrator/GetPesanan";
         $http({
             method: 'GET',
             url: url,
         }).success(
             function (data, status, header, cfg) {
                 $scope.Pesanan = data;
             }


         ).error(function (err, status) {
             alert(err.Message + ", " + status);
         });
     }

    
 })

 .controller('PembayaranController', function ($scope, $http) {
     $scope.Judul = "Pembayaran";
     $scope.Init=function()
     {
         $scope.Categories = [];
         var url = "/api/Administrator/GetPembayaran";
         $http({
             method: 'GET',
             url: url,
         }).success(
             function (data, status, header, cfg) {
                 $scope.Pembayaran = data;
             }


         ).error(function (err, status) {
             alert(err.Message + ", " + status);
         });
     }

     $scope.ChangeValidate = function (item,data) {
         var old = item.VerifikasiPembayaran;
         item.VerifikasiPembayaran = data;

         var url = "/api/Administrator/ValidatePayment";
         $http({
             method: 'Post',
             url: url,
             data: item
         }).success(
             function (data, status, header, cfg) {
                 alert("Success Insert Data");
             }

         ).error(function (err, status) {
             alert(err.Message);
             item.ValidatePayment = old;
         });
     }



     $scope.ShowImage = function (data) {
         $scope.Selected = data;
         $scope.Selected.DefaultPembayaran = data.Pemesanan.VerifikasiPembayaran;
         $scope.Selected.ImageData = "data:image/png;base64,"+data.Pembayaran.data;
     }
 })


 .controller('LaporanPenjualanController', function ($scope, $http) {
     $scope.Judul = "LAPORAN PENJUALAN";
     
     $scope.Init=function()
     {
         $scope.Penjualan = [];
         $scope.TotalBiaya = 0;
         $scope.TotalFee = 0;

         var url = "/api/Administrator/GetPenjualan";
         $http({
             method: 'GET',
             url: url,
         }).success(function (data, status, header, cfg) {
             $scope.Penjualan = data;
             angular.forEach(data, function (value,key) {
                 $scope.TotalBiaya += value.biaya;
                 $scope.TotalFee += value.fee;
             })

             }
         ).error(function (err, status) {
             alert(err.Message + ", " + status);
         });
     }
 })




;

