// TODO: Minify and bundle CSS
const gulp = require("gulp"),
  less = require("gulp-less"),
  clean = require("gulp-clean");

const paths = {};
paths.webroot = "./wwwroot/";
paths.cssTarget = paths.webroot + "css/";
paths.lessSrc = paths.webroot + "styles/**/*.less";

gulp.task("compile:less", function () {
  return gulp
    .src(paths.lessSrc)
    .pipe(less())
    .pipe(gulp.dest(paths.cssTarget));
});

gulp.task("clean:css", function () {
  return gulp.src(paths.cssTarget + "*.css", { read: false }).pipe(clean());
});

gulp.task("process:less", gulp.series("clean:css", "compile:less"));

gulp.task("watch:less", function () {
  gulp.watch(paths.lessSrc, gulp.series(["process:less"]));
});

gulp.task("default", gulp.series("process:less"));
