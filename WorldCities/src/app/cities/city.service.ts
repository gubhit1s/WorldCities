import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../base.service';
import { Observable } from 'rxjs';

import { City } from './city';

@Injectable({
  providedIn: 'root'
})

export class CityService extends BaseService<City> {
  constructor(http: HttpClient) {
    super(http);
  }

  getData(pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string | null,
    filterQuery: string | null
  ): Observable<ApiResult<City>> {
    var url = this.getUrl("api/Cities");
    var params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("sortColumn", sortColumn)
      .set("sortOrder", sortOrder);

    if (filterColumn && filterQuery) {
      params = params.set("filterColumn", filterColumn)
        .set("filterQuery", filterQuery);
    }
    return this.http.get<ApiResult<City>>(url, { params });
  }

  get(id: number): Observable<City> {
    var url = this.getUrl("api/cities/" + id);
    return this.http.get<City>(url);
  }

  put(item: City): Observable<City> {
    var url = this.getUrl("api/cities/" + item.id);
    return this.http.put<City>(url, item);
  }

  post(item: City): Observable<City> {
    var url = this.getUrl("api/cities");
    return this.http.post<City>(url, item);
  }

}
