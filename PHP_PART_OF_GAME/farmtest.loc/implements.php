<?php

class User implements \JsonSerializable
{
    public $normal;
    public $json;

    public function __construct($normal, $json)
    {
        $this->normal = $normal;
        $this->json = $json;
    }

    public function jsonSerialize()
    {
        return get_object_vars($this);
    }
}

class ListOfTrades implements \JsonSerializable
{
    public $usersids = array();
    public $usersjsons = array();
    public $list = array();
    public $start = 0;
    public function __construct($list, $usersids, $usersjsons, $start)
    {
        $this->usersids = $usersids;
        $this->usersjsons = $usersjsons;
        $this->list = $list;
        $this->start = $start;
    }

    public function jsonSerialize()
    {
        return get_object_vars($this);
    }
}

?>