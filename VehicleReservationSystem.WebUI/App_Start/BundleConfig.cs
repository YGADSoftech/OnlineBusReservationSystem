
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Resolvers;
using System.Web;
using System.Web.Optimization;

namespace VehicleReservationSystem.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-3.2.1.min.js",
                        "~/Scripts/bootstrap4/popper.js",
                        "~/Scripts/bootstrap4/bootstrap.min.js",
                        "~/Scripts/owl.carousel.js",
                        "~/Scripts/isotope.pkgd.min.js",
                        "~/Scripts/jquery.scrollTo.min.js",
                        "~/Scripts/easing.js",
                        "~/Scripts/parallax.min.js",
                        "~/Scripts/ScrollToPlugin.min.js",
                        "~/Scripts/animation.gsap.min.js",
                        "~/Scripts/ScrollMagic.min.js",
                        "~/Scripts/TimelineMax.min.js",
                        "~/Scripts/TweenMax.min.js",
                        "~/Scripts/about.js",
                        "~/Scripts/contact.js",
                        

                        "~/Scripts/custom.js"





                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/styles/bootstrap4/bootstrap.min.css",
                      //"~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/owl.carousel.css",
                      "~/Content/owl.theme.default.css",
                      "~/Content/animate.css",
                      "~/Content/main_styles.css",
                      "~/Content/about.css",
                      "~/Content/about_responsive.css",
                      "~/Content/contact.css",
                      "~/Content/contact_responsive.css",
                      "~/Content/responsive.css"
                      
                      ));

            var nullBulider = new NullBuilder();
            var nullOrderer = new NullOrderer();

            BundleResolver.Current = new CustomBundleResolver();
            var commonStyleBundle = new CustomStyleBundle("~/Bundle/sass");

            commonStyleBundle.Include("~/css/seat.scss");
            commonStyleBundle.Orderer = nullOrderer;
            bundles.Add(commonStyleBundle);



        }
    }
}
