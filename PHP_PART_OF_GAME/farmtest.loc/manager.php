<?php include("include/config.php");  include("implements.php");



if (isset($_POST['lg_go'])){
    if ($_POST['login'] != ""){
        if ($_POST['password'] != ""){
            //print("SELECT * FROM `users` WHERE `login` = '{$_POST['Login']}' AND `password` = '{$_POST['Password']}' ORDER BY `id` LIMIT 1");
            $lg =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['login']);
            $ps =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['password']);

            $query = mysqli_query($connections, "SELECT * FROM `users` WHERE `login` = '{$lg}' AND `password` = '{$ps}' ORDER BY `id` LIMIT 1");
            if (mysqli_num_rows($query) > 0){
                $fetch = mysqli_fetch_assoc($query);
                $user = new User(true, $fetch['json']);
                echo(json_encode($user->jsonSerialize()));
                die;
            }
            $user = new User(false, "Error");
            echo(json_encode($user->jsonSerialize()));
            die;
        }
    }
    
    $user = new User(false, "Error");
    echo(json_encode($user->jsonSerialize()));
}

if (isset($_POST['reg_go'])){
    if ($_POST['login'] != ""){
        if ($_POST['password'] != ""){
            //print("SELECT * FROM `users` WHERE `login` = '{$_POST['Login']}' AND `password` = '{$_POST['Password']}' ORDER BY `id` LIMIT 1");
            $lg =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['login']);
            $ps =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['password']);

            $get = mysqli_query($connections, "SELECT * FROM `users` WHERE `login` = '{$lg}' ORDER BY `id`");

            if (mysqli_num_rows($get) == 0){
                
                $query = mysqli_query($connections, "INSERT INTO `users`(`login`, `password`, `json`) VALUES ('{$lg}','$ps','')");
                $user = new User(true, "");
                echo(json_encode($user->jsonSerialize()));
                die;
            }
            $user = new User(false, "Login exists");
            echo(json_encode($user->jsonSerialize()));
            die;
        }
    }
    $user = new User(false, "Error");
    echo(json_encode($user->jsonSerialize()));
}
if (isset($_POST['save'])){
    if ($_POST['login'] != ""){
        if ($_POST['password'] != ""){
            //print("SELECT * FROM `users` WHERE `login` = '{$_POST['Login']}' AND `password` = '{$_POST['Password']}' ORDER BY `id` LIMIT 1");
            $lg =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['login']);
            $ps =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['password']);

            $query = mysqli_query($connections, "UPDATE `users` SET `json`='{$_POST['json']}' WHERE `password` = '{$ps}' AND `login` ='{$lg}'");
            if (mysqli_error($connections) == null){
                $max = mysqli_fetch_assoc(mysqli_query($connections, "SELECT MAX(`id`) FROM `users`"))['MAX(`id`)'];

                $user = new User(true, "{$max}");
                echo(json_encode($user->jsonSerialize()));
                die;
            }else{
                $user = new User(false, "Error");
                echo(json_encode($user->jsonSerialize()));
            }

        }
    }
}

if (isset($_POST['setShop'])){
    if (isset($_POST['json'])){
        $lg =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['login']);
        $ps =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['password']);
        
        $user = mysqli_fetch_assoc(mysqli_query($connections, "SELECT * FROM `users` WHERE `login` = '{$lg}' ORDER BY `id`"));

        mysqli_query($connections, "DELETE FROM `trades` WHERE `userid` = {$user['id']}");

        mysqli_query($connections, "INSERT INTO `trades`(`userid`, `json`) VALUES ({$user['id']},'{$_POST['json']}')");
    }
}
if (isset($_POST['getShop'])){
    $lg =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['login']);
    $ps =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['password']);
    $user = mysqli_fetch_assoc(mysqli_query($connections, "SELECT * FROM `users` WHERE `login` = '{$lg}' ORDER BY `id`"));


    $shop = mysqli_fetch_assoc(mysqli_query($connections, "SELECT * FROM `trades` WHERE `userid` = {$user['id']}"));
    
    if ($shop != false){
        $user = new User(true, $shop['json']);
        echo(json_encode($user->jsonSerialize()));
    }else{
        $user = new User(false, "Null");
        echo(json_encode($user->jsonSerialize()));
    }
}
if (isset($_POST['removeShop'])){
        $lg =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['login']);
        $ps =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['password']);
        
        $user = mysqli_fetch_assoc(mysqli_query($connections, "SELECT * FROM `users` WHERE `login` = '{$lg}' ORDER BY `id`"));

        mysqli_query($connections, "DELETE FROM `trades` WHERE `userid` = {$user['id']}");
}

if (isset($_POST['getList'])){
    
    $lg =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['login']);
    $ps =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['password']);
    $user = mysqli_fetch_assoc(mysqli_query($connections, "SELECT * FROM `users` WHERE `login` = '{$lg}' ORDER BY `id`"));

    $max = mysqli_fetch_assoc(mysqli_query($connections, "SELECT MAX(`id`) FROM `users`"))['MAX(`id`)'];
    $start = random_int(0, intval($max));
    
    $min = $start - 10;
    if ($min < 0){
        $min = 0;
    }
    $list = mysqli_query($connections, "SELECT * FROM `trades` WHERE `id`!={$user['id']} ORDER BY `id` LIMIT  20 OFFSET {$min} ");
    $usersid = array();
    $jsons = array();
    $ls = array();
    while ($item = mysqli_fetch_assoc($list)) {
        if ($item['userid'] != $user['id']){
            array_push($usersid, $item['userid']);
            $json = mysqli_fetch_assoc(mysqli_query($connections,"SELECT * FROM `users` WHERE `id` = {$item['userid']}"));
            array_push($jsons, $json['json']);
            array_push($ls, $item['json']);
        }   
    }
    $tradelist = new ListOfTrades($ls,$usersid, $jsons, $start);

    $user = new User(true, $tradelist);
    echo(json_encode($tradelist->jsonSerialize()));
}


if (isset($_POST['upList'])){
    
    $lg =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['login']);
    $ps =  preg_replace("/[^A-Za-z0-9?! ]/","",$_POST['password']);
    $user = mysqli_fetch_assoc(mysqli_query($connections, "SELECT * FROM `users` WHERE `login` = '{$lg}' ORDER BY `id`"));

    $max = mysqli_fetch_assoc(mysqli_query($connections, "SELECT MAX(`id`) FROM `users`"))['MAX(`id`)'];
    $start = intval($_POST['start']);
    
    $min = $start - 10;
    if ($min < 0){
        $min = 0;
    }
    $list = mysqli_query($connections, "SELECT * FROM `trades` WHERE `id`!={$user['id']} ORDER BY `id` LIMIT  20 OFFSET {$min} ");
    $usersid = array();
    $jsons = array();
    $ls = array();
    while ($item = mysqli_fetch_assoc($list)) {
        if ($item['userid'] != $user['id']){
            array_push($usersid, $item['userid']);
            $json = mysqli_fetch_assoc(mysqli_query($connections,"SELECT * FROM `users` WHERE `id` = {$item['userid']}"));
            array_push($jsons, $json['json']);
            array_push($ls, $item['json']);
        }   
    }
    $tradelist = new ListOfTrades($ls,$usersid, $jsons, $start);

    $user = new User(true, $tradelist);
    echo(json_encode($tradelist->jsonSerialize()));
}
if (isset($_POST['byInList'])){
    $traderow = mysqli_query($connections, "SELECT * FROM `trades` WHERE `userid`={$_POST['userid']} ORDER BY `id`");

    if ($traderow != false){
        $data = json_decode(mysqli_fetch_assoc($traderow)['json']);
        $data->items[intval($_POST['slot'])]->selled = true;
        $data = json_encode($data);
        mysqli_query($connections, "UPDATE `trades` SET `json`='{$data}' WHERE `userid`={$_POST['userid']}");
        print_r("TRADES: " . $data);
    }
}
?>