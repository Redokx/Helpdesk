/// <binding AfterBuild='default' />
//const gulp = require('gulp');
//const sass = require('gulp-sass');
//const browserSync = require('browser-sync').create();
//const autoprefixer = require('gulp-autoprefixer');

//sass.compiler = require('node-sass');

//gulp.task('sass', function () {
//    return gulp.src(['./Content/sass/**/*.scss'])
//        .pipe(sass().on('error', sass.logError))
//        .pipe(autoprefixer({
//            browsers: ['last 2 versions'],
//            cascade: false
//        }))
//        .pipe(gulp.dest('./Content/css'))
//        .pipe(browserSync.stream());
//});

//gulp.task('serve', function () {

//    browserSync.init({
//        server: ["./", "./src"]
//    });

//    gulp.watch("./Content/sass/**/*.scss", gulp.series('sass'));
//    gulp.watch("./Content/**/*.js").on('change', browserSync.reload);
//    gulp.watch("./Content/*.html").on('change', browserSync.reload);
//});



//gulp.task('default', gulp.series('serve'));


const gulp = require('gulp');
const browserSync = require('browser-sync').create();
const autoprefixer = require('gulp-autoprefixer');

var sass = require('gulp-sass')(require('sass'));
gulp.task('build-css', function () {
    return gulp
        .src('./Content/SASS/*.scss')
        .pipe(sass())
        .pipe(gulp.dest('./Content/CSS'));
});

gulp.task('default', gulp.series('build-css'));