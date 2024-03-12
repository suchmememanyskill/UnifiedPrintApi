using Newtonsoft.Json;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.Printables.Model;
using Utils;

namespace UnifiedPrintApi.Service.Printables;

public record PrintablesSortType(string Name, string UrlPart)
{
    public string InternalName => Name.ToLower().Replace(' ', '-');
}

public class PrintablesApi : IApiDescription
{
    private static readonly string BASE_URL = "https://api.printables.com/graphql/";
    
    public string Name => "Prusa Printables";
    public string Color => "#FA6831";
    public List<SortType> SortTypes => ActualSortTypes.Select(x => new SortType(x.Name, x.InternalName, null)).ToList();

    public static readonly List<PrintablesSortType> ActualSortTypes = new()
    {
        new("Newest", "-first_publish"),
        new("Random", "random"),
        new("Featured", "-"),
        new("Most downloaded last 7 days", "-download_count_7_days"),
        new("Most downloaded last 7 days only new", "-download_count_7_days"),
        new("Most downloaded last 30 days", "-download_count_30_days"),
        new("Most downloaded last 30 days only new", "-download_count_30_days"),
        new("Most downloaded all time", "-download_count"),
        new("Most liked last 7 days", "-likes_count_7_days"),
        new("Most liked last 7 days only new", "-likes_count_7_days"),
        new("Most liked last 30 days", "-likes_count_30_days"),
        new("Most liked last 30 days only new", "-likes_count_30_days"),
        new("Most liked all time", "-likes_count"),
        new("Most makes last 7 days", "-makes_count_7_days"),
        new("Most makes last 30 days", "-makes_count_30_days"),
        new("Most makes all time", "-makes_count"),
        new("Most viewed last 7 days", "-display_count_7_days"),
        new("Most viewed last 30 days", "-display_count_30_days"),
        new("Most viewed all time", "-display_count"),
        new("Top rated last 7 days", "-rating_avg_7_days"),
        new("Top rated last 30 days", "-rating_avg_30_days"),
        new("Top rated all time", "-rating_avg"),
    };

    public Uri Site => new("https://www.printables.com/");
    public string Description => "Provides access to the Printables API";
    private string GetTemplate => new("{\"operationName\":\"PrintList\",\"variables\":{\"limit\":{{LIMIT}},\"categoryId\":null,\"publishedDateLimitDays\":{{PUBLISHDATE}},\"hasMake\":false,\"competitionAwarded\":false,\"featured\":false,\"likedByMe\":false,\"collectedByMe\":false,\"madeByMe\":false,\"ordering\":\"{{DISPLAY}}\",\"cursor\":{{CURSOR}}},\"query\":\"query PrintList($limit: Int!, $cursor: String, $categoryId: ID, $materialIds: [Int], $userId: ID, $printerIds: [Int], $licenses: [ID], $ordering: String, $hasModel: Boolean, $filesType: [FilterPrintFilesTypeEnum], $includeUserGcodes: Boolean, $nozzleDiameters: [Float], $weight: IntervalObject, $printDuration: IntervalObject, $publishedDateLimitDays: Int, $featured: Boolean, $featuredNow: Boolean, $usedMaterial: IntervalObject, $hasMake: Boolean, $competitionAwarded: Boolean, $onlyFollowing: Boolean, $collectedByMe: Boolean, $madeByMe: Boolean, $likedByMe: Boolean) {\\n  morePrints(\\n    limit: $limit\\n    cursor: $cursor\\n    categoryId: $categoryId\\n    materialIds: $materialIds\\n    printerIds: $printerIds\\n    licenses: $licenses\\n    userId: $userId\\n    ordering: $ordering\\n    hasModel: $hasModel\\n    filesType: $filesType\\n    nozzleDiameters: $nozzleDiameters\\n    includeUserGcodes: $includeUserGcodes\\n    weight: $weight\\n    printDuration: $printDuration\\n    publishedDateLimitDays: $publishedDateLimitDays\\n    featured: $featured\\n    featuredNow: $featuredNow\\n    usedMaterial: $usedMaterial\\n    hasMake: $hasMake\\n    onlyFollowing: $onlyFollowing\\n    competitionAwarded: $competitionAwarded\\n    collectedByMe: $collectedByMe\\n    madeByMe: $madeByMe\\n    liked: $likedByMe\\n  ) {\\n    cursor\\n    items {\\n      ...PrintListFragment\\n      printer {\\n        id\\n        __typename\\n      }\\n      user {\\n        rating\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\\nfragment PrintListFragment on PrintType {\\n  id\\n  name\\n  slug\\n  ratingAvg\\n  ratingCount\\n  likesCount\\n  liked\\n  datePublished\\n  dateFeatured\\n  firstPublish\\n  downloadCount\\n  displayCount\\n  inMyCollections\\n  foundInUserGcodes\\n  userGcodeCount\\n  userGcodesCount\\n  materials {\\n    id\\n    __typename\\n  }\\n  category {\\n    id\\n    path {\\n      id\\n      name\\n      __typename\\n    }\\n    __typename\\n  }\\n  modified\\n  images {\\n    ...ImageSimpleFragment\\n    __typename\\n  }\\n  filesType\\n  hasModel\\n  user {\\n    ...AvatarUserFragment\\n    __typename\\n  }\\n  ...LatestCompetitionResult\\n  __typename\\n}\\n\\nfragment AvatarUserFragment on UserType {\\n  id\\n  publicUsername\\n  avatarFilePath\\n  slug\\n  badgesProfileLevel {\\n    profileLevel\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment LatestCompetitionResult on PrintType {\\n  latestCompetitionResult {\\n    placement\\n    competitionId\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment ImageSimpleFragment on PrintImageType {\\n  id\\n  filePath\\n  rotation\\n  __typename\\n}\\n\"}");
    private string ModelTemplate => new("{\"operationName\":\"PrintProfile\",\"variables\":{\"id\":\"{{ID}}\",\"loadPurchase\":false},\"query\":\"query PrintProfile($id: ID!, $loadPurchase: Boolean!) {\\n  print(id: $id) {\\n    ...PrintDetailFragment\\n    price\\n    user {\\n      billingAccountType\\n      lowestTierPrice\\n      highlightedModels {\\n        models {\\n          ...PrintListFragment\\n          __typename\\n        }\\n        featured\\n        __typename\\n      }\\n      __typename\\n    }\\n    purchaseDate @include(if: $loadPurchase)\\n    paidPrice @include(if: $loadPurchase)\\n    __typename\\n  }\\n}\\n\\nfragment PrintDetailFragment on PrintType {\\n  id\\n  slug\\n  name\\n  authorship\\n  remixDescription\\n  premium\\n  price\\n  excludeCommercialUsage\\n  eduProject {\\n    id\\n    subject {\\n      id\\n      name\\n      slug\\n      __typename\\n    }\\n    language {\\n      id\\n      name\\n      __typename\\n    }\\n    free\\n    timeDifficulty\\n    audienceAge\\n    complexity\\n    equipment {\\n      id\\n      name\\n      __typename\\n    }\\n    suitablePrinters {\\n      id\\n      name\\n      __typename\\n    }\\n    organisation\\n    authors\\n    targetGroupFocus\\n    knowledgeAndSkills\\n    objectives\\n    equipmentDescription\\n    timeSchedule\\n    workflow\\n    approved\\n    datePublishRequested\\n    __typename\\n  }\\n  user {\\n    ...AvatarUserFragment\\n    isFollowedByMe\\n    canBeFollowed\\n    email\\n    publishedPrintsCount\\n    premiumPrintsCount\\n    designer\\n    stripeAccountActive\\n    membership {\\n      currentTier {\\n        id\\n        name\\n        benefits {\\n          id\\n          title\\n          benefitType\\n          description\\n          __typename\\n        }\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  ratingAvg\\n  myRating\\n  ratingCount\\n  description\\n  category {\\n    id\\n    path {\\n      id\\n      name\\n      storeName\\n      description\\n      storeDescription\\n      __typename\\n    }\\n    __typename\\n  }\\n  modified\\n  firstPublish\\n  datePublished\\n  dateCreatedThingiverse\\n  nsfw\\n  summary\\n  shareCount\\n  likesCount\\n  makesCount\\n  liked\\n  printDuration\\n  numPieces\\n  weight\\n  nozzleDiameters\\n  usedMaterial\\n  layerHeights\\n  materials {\\n    id\\n    name\\n    __typename\\n  }\\n  dateFeatured\\n  downloadCount\\n  displayCount\\n  filesCount\\n  privateCollectionsCount\\n  publicCollectionsCount\\n  pdfFilePath\\n  commentCount\\n  userGcodeCount\\n  remixCount\\n  canBeRated\\n  printer {\\n    id\\n    name\\n    __typename\\n  }\\n  image {\\n    ...ImageSimpleFragment\\n    __typename\\n  }\\n  images {\\n    ...ImageSimpleFragment\\n    __typename\\n  }\\n  tags {\\n    name\\n    id\\n    __typename\\n  }\\n  thingiverseLink\\n  filesType\\n  license {\\n    id\\n    disallowRemixing\\n    __typename\\n  }\\n  remixParents {\\n    ...remixParentDetail\\n    __typename\\n  }\\n  gcodes {\\n    id\\n    name\\n    fileSize\\n    filePreviewPath\\n    __typename\\n  }\\n  stls {\\n    id\\n    name\\n    fileSize\\n    filePreviewPath\\n    __typename\\n  }\\n  slas {\\n    id\\n    name\\n    fileSize\\n    filePreviewPath\\n    __typename\\n  }\\n  ...LatestCompetitionResult\\n  competitions {\\n    id\\n    name\\n    slug\\n    description\\n    isOpen\\n    __typename\\n  }\\n  competitionResults {\\n    placement\\n    competition {\\n      id\\n      name\\n      slug\\n      printsCount\\n      openFrom\\n      openTo\\n      __typename\\n    }\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment AvatarUserFragment on UserType {\\n  id\\n  publicUsername\\n  avatarFilePath\\n  handle\\n  company\\n  verified\\n  badgesProfileLevel {\\n    profileLevel\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment ImageSimpleFragment on PrintImageType {\\n  id\\n  filePath\\n  rotation\\n  __typename\\n}\\n\\nfragment remixParentDetail on PrintRemixType {\\n  id\\n  parentPrintId\\n  parentPrintName\\n  parentPrintAuthor {\\n    id\\n    slug\\n    publicUsername\\n    company\\n    verified\\n    handle\\n    __typename\\n  }\\n  parentPrint {\\n    id\\n    name\\n    slug\\n    datePublished\\n    image {\\n      ...ImageSimpleFragment\\n      __typename\\n    }\\n    premium\\n    authorship\\n    license {\\n      id\\n      name\\n      disallowRemixing\\n      __typename\\n    }\\n    eduProject {\\n      id\\n      __typename\\n    }\\n    __typename\\n  }\\n  url\\n  urlAuthor\\n  urlImage\\n  urlTitle\\n  urlLicense {\\n    id\\n    name\\n    disallowRemixing\\n    __typename\\n  }\\n  urlLicenseText\\n  __typename\\n}\\n\\nfragment LatestCompetitionResult on PrintType {\\n  latestCompetitionResult {\\n    placement\\n    competitionId\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment PrintListFragment on PrintType {\\n  id\\n  name\\n  slug\\n  ratingAvg\\n  likesCount\\n  liked\\n  datePublished\\n  dateFeatured\\n  firstPublish\\n  downloadCount\\n  category {\\n    id\\n    path {\\n      id\\n      name\\n      __typename\\n    }\\n    __typename\\n  }\\n  modified\\n  image {\\n    ...ImageSimpleFragment\\n    __typename\\n  }\\n  nsfw\\n  premium\\n  price\\n  user {\\n    ...AvatarUserFragment\\n    __typename\\n  }\\n  ...LatestCompetitionResult\\n  __typename\\n}\"}");
    private string SearchTemplate => new("{\"operationName\":\"SearchPrints\",\"variables\":{\"query\":\"{{QUERY}}\",\"ordering\":null,\"limit\":{{LIMIT}},\"offset\":{{OFFSET}}},\"query\":\"query SearchPrints($query: String, $limit: Int, $offset: Int, $ordering: SearchChoicesEnum) {\\n  result: searchPrints(\\n    query: $query\\n    printType: print\\n    limit: $limit\\n    offset: $offset\\n    ordering: $ordering\\n  ) {\\n    items {\\n      ...SearchPrintsFragment\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\\nfragment SearchPrintsFragment on SearchPrintType {\\n  id\\n  score\\n  explanation\\n  mainImage\\n  ratingAvg\\n  ratingCount\\n  filesType\\n  hasModel\\n  name\\n  ...LatestCompetitionResultSearch\\n  downloadCount\\n  displayCount\\n  dateFeatured\\n  datePublished\\n  slug\\n  liked\\n  likesCount\\n  user {\\n    ...AvatarSearchFragment\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment AvatarSearchFragment on SearchUserType {\\n  id\\n  slug\\n  publicUsername\\n  avatarFilePath\\n  badgesProfileLevel {\\n    profileLevel\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment LatestCompetitionResultSearch on SearchPrintType {\\n  latestCompetitionResult {\\n    placement\\n    competitionId\\n    __typename\\n  }\\n  __typename\\n}\\n\"}");
    private string FeaturedTemplate => new("{\"operationName\":\"FeaturedPrints\",\"variables\":{\"limit\":36,\"offset\":0},\"query\":\"query FeaturedPrints($limit: Int!, $offset: Int) {\\n  prints(\\n    limit: $limit\\n    offset: $offset\\n    ordering: \\\"-date_published\\\"\\n    featuredNow: true\\n  ) {\\n    ...PrintListFragment\\n    __typename\\n  }\\n}\\n\\nfragment PrintListFragment on PrintType {\\n  id\\n  name\\n  slug\\n  ratingAvg\\n  likesCount\\n  liked\\n  datePublished\\n  dateFeatured\\n  firstPublish\\n  downloadCount\\n  category {\\n    id\\n    path {\\n      id\\n      name\\n      __typename\\n    }\\n    __typename\\n  }\\n  modified\\n  image {\\n    ...ImageSimpleFragment\\n    __typename\\n  }\\n  nsfw\\n  premium\\n  user {\\n    ...AvatarUserFragment\\n    __typename\\n  }\\n  ...LatestCompetitionResult\\n  __typename\\n}\\n\\nfragment AvatarUserFragment on UserType {\\n  id\\n  publicUsername\\n  avatarFilePath\\n  handle\\n  company\\n  verified\\n  badgesProfileLevel {\\n    profileLevel\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment LatestCompetitionResult on PrintType {\\n  latestCompetitionResult {\\n    placement\\n    competitionId\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment ImageSimpleFragment on PrintImageType {\\n  id\\n  filePath\\n  rotation\\n  __typename\\n}\"}");
    private string DownloadTemplate => new("{\"operationName\":\"GetDownloadLink\",\"variables\":{\"id\":\"{{FILE_ID}}\",\"fileType\":\"{{FILE_TYPE}}\",\"printId\":\"{{POST_ID}}\",\"source\":\"model_detail\"},\"query\":\"mutation GetDownloadLink($id: ID!, $printId: ID!, $fileType: DownloadFileTypeEnum!, $source: DownloadSourceEnum!) {\\n  getDownloadLink(\\n    id: $id\\n    printId: $printId\\n    fileType: $fileType\\n    source: $source\\n  ) {\\n    ok\\n    errors {\\n      field\\n      messages\\n      __typename\\n    }\\n    output {\\n      link\\n      count\\n      ttl\\n      __typename\\n    }\\n    __typename\\n  }\\n}\"}");
    
    private Cache _cache;

    public PrintablesApi(Cache cache)
    {
        _cache = cache;
    }
    
    public IApiPreviewPosts GetPosts(SortType type, int page, int perPage)
    {
        PrintablesSortType? ptype = ActualSortTypes.Find(x => type.DisplayName == x.Name);
        if (ptype == null)
            throw new Exception("Type not found");

        if (ptype.Name == "Featured")
            return GetPostsByFeatured(page, perPage);
        
        string dateLimit = "null";
        string ordering = ptype.UrlPart;

        if (ptype.Name.Contains("only new"))
        {
            if (ptype.Name.Contains("7 days"))
                dateLimit = "7";
            else if (ptype.Name.Contains("30 days"))
                dateLimit = "30";
        }
        
        string cursor = "null";

        if (page > 1)
        {
            string key = $"{dateLimit}:{perPage}:{page}:{ordering}";
            string hash = Cache.Hash(key);
            
            cursor = _cache.CacheValue<string>(hash, () => throw new Exception(
                "Previous page was never loaded. Please load all previous pages before this one before requesting this page"))!;
        }
        
        string template = GetTemplate;

        string json = template
            .Replace("{{LIMIT}}", perPage.ToString())
            .Replace("{{PUBLISHDATE}}", dateLimit)
            .Replace("{{CURSOR}}", cursor)
            .Replace("{{DISPLAY}}", ordering);

        string response;
        try
        {
            response = Request.PostString(new Uri(BASE_URL), json);
        }
        catch
        {
            return new GenericApiPreviewPosts(new List<PrintablesPreviewPost>(), 0);
        }

        PrintList prints = JsonConvert.DeserializeObject<PrintList>(response);
        string nextKey = $"{dateLimit}:{perPage}:{page + 1}:{ordering}";
        _cache.AddCacheValue(Cache.Hash(nextKey), $"\"{prints.Data.MorePrints.Cursor}\"");
        return new GenericApiPreviewPosts(prints.Data.MorePrints.Items.Select(x => new PrintablesPreviewPost(this, x)));
    }

    public IApiPreviewPosts GetPostsBySearch(string search, int page, int perPage)
    {
        string template = SearchTemplate;
        string json = template
            .Replace("{{QUERY}}", search)
            .Replace("{{LIMIT}}", perPage.ToString())
            .Replace("{{OFFSET}}", ((page - 1) * perPage).ToString());

        string response;
        try
        {
            response = Request.PostString(new Uri(BASE_URL), json);
        }
        catch
        {
            return new GenericApiPreviewPosts(new List<PrintablesPreviewPost>(), 0);
        }
         
            
        PrintList prints = JsonConvert.DeserializeObject<PrintList>(response);
        return new GenericApiPreviewPosts(prints.Data.SearchResult.Items.Select(x => new PrintablesPreviewPost(this, x)));
    }

    public IApiPreviewPosts GetPostsByFeatured(int page, int perPage)
    {
        if (page <= 0)
            throw new Exception("Invalid page number");
        
        string hash = Cache.Hash("Printables-Featured");
        string response;
        try
        {
            response = _cache.CacheValue(hash, () => Request.PostString(new Uri(BASE_URL), FeaturedTemplate))!;
        }
        catch
        {
            return new GenericApiPreviewPosts(new List<PrintablesPreviewPost>(), 0);
        }
        
        PrintList prints = JsonConvert.DeserializeObject<PrintList>(response)!;
        return new GenericApiPreviewPosts(prints.Data.FeaturedPrints.Skip((page - 1) * perPage).Take(perPage).Select(x => new PrintablesPreviewPost(this, x)));
    }

    public IApiPost? GetPostById(string id)
    {
        string template = ModelTemplate;
        string json = template.Replace("{{ID}}", id);

        string response;
        try
        {
            response = Request.PostString(new Uri(BASE_URL), json);
        }
        catch
        {
            return null;
        }
         

        PrintModelData model = JsonConvert.DeserializeObject<PrintModelData>(response);

        if (model.Data.Print == null)
            return null;
        
        return new PrintablesPost(this, model!.Data.Print);
    }

    private readonly Dictionary<string, string> conv = new()
    {
        { "STLType", "stl" }
    };
    
    public string? GetDownloadLink(string fileType, string fileId, string postId)
    {
        try
        {
            string template = DownloadTemplate.Replace("{{FILE_ID}}", fileId).Replace("{{FILE_TYPE}}", conv[fileType])
                .Replace("{{POST_ID}}", postId);
            
            string response = Request.PostString(new Uri(BASE_URL), template);
            DownloadModelRoot model = JsonConvert.DeserializeObject<DownloadModelRoot>(response);

            if (model?.Data?.Data?.Ok ?? false)
            {
                return model.Data?.Data.Output.Link;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }

        return null;
    }
}