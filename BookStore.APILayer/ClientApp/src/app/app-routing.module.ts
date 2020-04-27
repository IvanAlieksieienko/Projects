import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { SharedService } from "./services/shared.service";
import { BookGetAllResolveService } from "./modules/book/bookGetAll/bookGetAllResolveService";
import { BookService } from "./services/book.service";
import { AuthorGetAllResolveService } from "./modules/author/authorGetAll/authorGetAllResolveService";
import { AuthorService } from "./services/author.service";
import { GenreGetAllResolveService } from "./modules/genre/genreGetAll/genreGetAllResolveService";
import { GenreService } from "./services/genre.service";
import { UserGetAllResolveService } from "./modules/user/userGetAll/userGetAllResolveService";
import { UserService } from "./services/user.service";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { HttpConfigInterceptor } from "./modules/Interceptor/httpconfig.interceptor";

const routes: Routes = [
        { path: '', loadChildren: "./modules/book/book.module#BookModule" },
        { path: 'authors', loadChildren: "./modules/author/author.module#AuthorModule" },
        { path: 'userbasket', loadChildren: "./modules/basketGetAll/basketGetAll.module#BasketGetAllModule" },
        { path: 'books', loadChildren: "./modules/book/book.module#BookModule" },
        { path: 'genres', loadChildren: "./modules/genre/genre.module#GenreModule" },
        { path: 'login', loadChildren: "./modules/login/login.module#LoginModule" },
        { path: 'management', loadChildren: "./modules/management/management.module#ManagementModule" },
        { path: 'profile', loadChildren: "./modules/profile/profile.module#ProfileModule" },
        { path: 'users', loadChildren: "./modules/user/user.module#UserModule" },
        { path: 'register', loadChildren: "./modules/register/register.module#RegisterModule" }
]

@NgModule({
        imports: [
                RouterModule.forRoot(routes),
        ],
        exports: [RouterModule],
        providers:
        [
                SharedService,
                BookGetAllResolveService,
                BookService,
                AuthorGetAllResolveService,
                AuthorService,
                GenreGetAllResolveService,
                GenreService,
                UserGetAllResolveService,
                UserService,
                { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }
        ]
})
export class AppRoutingModule { }