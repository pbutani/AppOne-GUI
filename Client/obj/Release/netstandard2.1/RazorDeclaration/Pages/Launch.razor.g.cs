#pragma checksum "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\Pages\Launch.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "912e271ad9c3bdd8d1721efbfad778da716cc097"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace BlazorApp1.Client.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\_Imports.razor"
using BlazorApp1.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\_Imports.razor"
using BlazorApp1.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\Pages\Launch.razor"
using Microsoft.AspNetCore.WebUtilities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\Pages\Launch.razor"
using System.Text;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\Pages\Launch.razor"
using BlazorApp1.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\Pages\Launch.razor"
using System.Diagnostics;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/launch")]
    public partial class Launch : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 16 "C:\Users\ekharebova\source\repos\BlazorApp1\BlazorApp1\Client\Pages\Launch.razor"
       
    private string currentUri = "";
    private string authUri = "";

    protected override async Task OnInitializedAsync()
    {
        var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
        currentUri = uri.ToString();
        //currentUri = "http://localhost:9147/FC_App/Launch?iss=https:%2F%2Ffhir-ehr.sandboxcerner.com%2Fr4%2F0b8a0111-e8e6-4c26-a91c-5069cbc6b1ca&launch=d13ae44f-8027-45de-8b72-ae5bb4207f1c";

        var qs = QueryHelpers.ParseQuery(uri.Query);
        if (qs.TryGetValue("iss", out var codeVal))
        {
            await Http.PostAsJsonAsync("Launch", currentUri);
            string[] vals = await Http.GetFromJsonAsync<string[]>("api/Values");
            authUri = vals[3];
            await jsRuntime.InvokeVoidAsync("open", authUri);
        }
        else
        {
            authUri = "";
        }
    }




#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime jsRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient Http { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavManager { get; set; }
    }
}
#pragma warning restore 1591
